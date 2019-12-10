using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour
{
    [SerializeField] private Vector3 destinationPos;

    [Range(0, 5)][SerializeField] private float speed = 0;
    private Vector3 originPos;
    private float timer = 0;
    private bool forward = true;
    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(forward) timer += (Time.deltaTime * speed);
        else timer -= (Time.deltaTime * speed);

        if (timer >= 1) forward = false;
        else if (timer <= 0) forward = true;
        transform.position = Vector3.Lerp(originPos, destinationPos,timer);
       //transform.position = new Vector3(transform.position.x,Mathf.PingPong(Time.time,6.5f )+ 1.5f, transform.position.z);
    }
}
