using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RocketScript : MonoBehaviour
{
    private enum State { alive, death, success }
    private bool Qpressed = false;
    private bool Dpressed = false;
    [SerializeField] private float force = 1500;
    [SerializeField] private float rotation = 150;
    private Rigidbody rb;

    private AudioSource aS;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip deathSound;

    [SerializeField] private ParticleSystem thrustParticle;
    [SerializeField] private ParticleSystem successParticle;
    [SerializeField] private ParticleSystem deathParticle;


    private State state = State.alive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aS = GetComponent<AudioSource>();
        state = State.alive;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.alive)
        {
            GoUp();
            LeftAndRight();
        }
    }

    void GoUp()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            if (!aS.isPlaying)
            {
                aS.Play();
                thrustParticle.Play();
            }
            rb.AddRelativeForce(force * Vector3.up * Time.deltaTime);
        }
        else
        {
            thrustParticle.Stop();
            aS.Stop();
        }
    }

    void LeftAndRight()
    {
        transform.eulerAngles = new Vector3(0, 180, transform.eulerAngles.z);
        if (Input.GetKey(KeyCode.Q) && !Dpressed)
        {
            rb.freezeRotation = true;
            Qpressed = true;
            //Debug.Log("Q Held");
            transform.Rotate(rotation * -Vector3.forward * Time.deltaTime);

        }
        else if (!Input.GetKey(KeyCode.Q)) Qpressed = false;
        rb.freezeRotation = false;
        if (Input.GetKey(KeyCode.D) && !Qpressed)
        {
            rb.freezeRotation = true;
            Dpressed = true;
            //Debug.Log("D Held");

            transform.Rotate(rotation * Vector3.forward * Time.deltaTime);

        }
        else if (!Input.GetKey(KeyCode.D)) Dpressed = false;
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state == State.alive)
        {
            switch (collision.gameObject.tag)
            {
                case "Safe":
                    //Start
                    break;
                case "End":
                    state = State.success;
                    aS.clip = successSound;
                    aS.Play();
                    successParticle.Play();
                    if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1)
                    {
                        Debug.Log("VICTOIRETAVU");
                        Time.timeScale = 0;
                    }
                    else
                    {
                        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, 3));
                    }
                    //Finish
                    break;
                default:
                    state = State.death;
                    aS.clip = deathSound;
                    aS.Play();
                    deathParticle.Play();
                    StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex, 3));
                    //Death
                    break;
            }
        }
    }

    IEnumerator LoadLevel(int levelName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
