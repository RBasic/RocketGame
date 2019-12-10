using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RocketScript : MonoBehaviour
{
    private bool Qpressed = false;
    private bool Dpressed = false;
    [SerializeField] private float force = 1500;
    [SerializeField] private float rotation = 150;
    private Rigidbody rb;
    private Transform trsf;
    private Vector3 originPos;
    private Vector3 originRot;

    private AudioSource aS;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip deathSound;

    enum State { alive, death, success}
    State state = State.alive;
    // Start is called before the first frame update
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trsf = GetComponent<Transform>();
        aS = GetComponent<AudioSource>();
        originPos = transform.position;
        originRot = transform.rotation.eulerAngles;
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

    
   private void resetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = originPos;
        transform.eulerAngles = originRot;
    }

    void GoUp()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!aS.isPlaying)
            {
                aS.Play();
            }
            //Debug.Log("Space Held");
            rb.AddRelativeForce(force * Vector3.up * Time.deltaTime);
        }
        else
        {
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
            trsf.Rotate(rotation * -Vector3.forward * Time.deltaTime);

        }
        else if (!Input.GetKey(KeyCode.Q)) Qpressed = false;
        rb.freezeRotation = false;
        if (Input.GetKey(KeyCode.D) && !Qpressed)
        {
            rb.freezeRotation = true;
            Dpressed = true;
            //Debug.Log("D Held");

            trsf.Rotate(rotation * Vector3.forward * Time.deltaTime);

        }
        else if (!Input.GetKey(KeyCode.D)) Dpressed = false;
        rb.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
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
                StartCoroutine(LoadLevel("Level2", 3));
                Debug.Log("Winner");
                //Finish
                break;
            default:
                state = State.death;
                aS.clip = deathSound;
                aS.Play();
                StartCoroutine(LoadLevel("Level1", 3));
                //Death
                break;
        }
    }

    IEnumerator LoadLevel(string levelName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
