using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       transform.position = new Vector3(transform.position.x,Mathf.PingPong(Time.time,6.5f )+ 1.5f, transform.position.z);
    }
}
