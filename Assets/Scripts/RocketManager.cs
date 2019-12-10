using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketManager : MonoBehaviour
{
    private bool bRight;
    private bool bLeft;
    private Vector3 originePosition;
    private Vector3 origineRotation;

    [SerializeField] private float thrusterSpeed = 1500;
    [SerializeField] private float rotationSpeed = 150;
    [SerializeField] private float fuel = 100;
    [SerializeField] private ParticleSystem ExplosionParticle;
    [SerializeField] private ParticleSystem SuccessParticle;
    [SerializeField] private ParticleSystem JetParticle;
    [SerializeField] private float fuelConsommation =  100;

    private AudioSource soundSource;
    [SerializeField] private AudioClip jetSound;
    [SerializeField] private AudioClip exploSound;
    [SerializeField] private AudioClip succcessSound;

    private bool accel = false;

    private Rigidbody rb;

    enum States
    {
        alive,
        death,
        success
    }


    private States currentState = States.alive;
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Level Start");
        soundSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        originePosition = transform.position;
        origineRotation = transform.eulerAngles;
        currentState = States.alive;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == States.alive)
        {
            if (accel)
            {
                fuel -= Time.deltaTime * fuelConsommation;
            }
            ThrusterRocket();
            Rotate();

        }
    }

    private void ThrusterRocket()
    {
        //Gestion touche espace


        if (Input.GetKey(KeyCode.Space))
        {
            if (!soundSource.isPlaying && !JetParticle.isPlaying)
            {
                soundSource.Play();
                JetParticle.Play();
            }
            if (fuel > 0)
            {
                rb.AddRelativeForce(thrusterSpeed * Vector3.up * Time.deltaTime);
                accel = true;
            }
            else
            {
                accel = false;
            }
        }
        else
        {
            soundSource.Stop();
            JetParticle.Stop();
        }
    }

    private void Rotate()
    {
        transform.eulerAngles = new Vector3(0, 180, transform.eulerAngles.z);

        
        //Gestion touche Q
        if (Input.GetKey(KeyCode.Q) && !bLeft)
        {
            rb.freezeRotation = true;
            RotateRocket(-1);
            bRight = true;
        }
        else if (!Input.GetKey(KeyCode.Q))
        {
            bRight = false;
        }

        // Gestion touche D
        if (Input.GetKey(KeyCode.D) && !bRight)
        {
            rb.freezeRotation = true;
            RotateRocket(1);
            bLeft = true;
        }
        else if (!Input.GetKey(KeyCode.D))
        {
            bLeft = false;
        }
        rb.freezeRotation = false;

    }

    private void RotateRocket(float dir)
    {
        transform.Rotate(rotationSpeed * dir * Vector3.forward * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == States.alive)
        {
            switch (collision.gameObject.tag)
            {
                case "FIN":
                    currentState = States.success;
                    soundSource.clip = succcessSound;
                    SuccessParticle.Play();
                    soundSource.Play();
                    StartCoroutine(LoadLevel("REVERU TWO", 1));
                    //
                    break;
                case "Untagged":
                    currentState = States.death;
                    soundSource.clip = exploSound;
                    ExplosionParticle.Play();
                    soundSource.Play();
                    StartCoroutine(LoadLevel("GAMU NO SAMA", 0.5f));
                    break;
                case "FUEL":
                    fuel += 50;
                    Destroy(collision.gameObject);
                    if (fuel >= 100)
                    {
                        fuel = 100;
                    }
                    break;
                default:
                    break;
            }
        }
        
    }

    
    IEnumerator LoadLevel(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
