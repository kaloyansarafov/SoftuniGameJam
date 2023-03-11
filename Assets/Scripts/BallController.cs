using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.Netcode;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    private float launchTime;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.GetComponentInParent<Rigidbody>().isKinematic = false;
    }


    public void recieveHit(Vector3 direction)
    {
        //push the ball in direction with rb
        GetComponentInParent<Rigidbody>().velocity = direction.normalized * 20;
        //make velocity slowly decrease
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //if(!IsServer)
        //   return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("hit player");
            if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 1f)
            {
                other.gameObject.GetComponent<RobotController>().isAlive.Value = false;
                gameObject.GetComponent<SphereCollider>().radius += 1f;
            }
        }
    }
}
