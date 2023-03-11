using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private float launchTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time - launchTime >= 1f)
        //{
        //    GetComponent<Rigidbody>().velocity -= GetComponent<Rigidbody>().velocity.normalized * 2 * Time.deltaTime;
        //}
    }
    public void recieveHit(Vector3 direction)
    {
        //push the ball in direction with rb
        GetComponent<Rigidbody>().velocity = direction.normalized * 20;
        //make velocity slowly decrease

    }
}
