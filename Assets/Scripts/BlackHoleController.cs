using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace StarterAssets
{
    public class BlackHoleController : NetworkBehaviour
    {
        [SerializeField] private float m_MovementSpeed = 3.0f;
        [SerializeField] private float m_Radius = 3.0f;

        //on collision with player when going faster than 3f, kill the player
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 3f)
                {
                    other.gameObject.GetComponent<RobotController>().isAlive.Value = false;
                }
            }
        }
    }
}