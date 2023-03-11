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
        if (Time.time - launchTime >= 1f)
        {
            GetComponent<Rigidbody>().velocity -= GetComponent<Rigidbody>().velocity.normalized * 2 * Time.deltaTime;
        }
    }
    public void recieveHit(Vector3 directon)
    {
        //push the ball at direction
        GetComponent<Rigidbody>().velocity = directon.normalized * 20;
        launchTime = Time.time;
        //make velocity slowly decrease
        
    }
}
