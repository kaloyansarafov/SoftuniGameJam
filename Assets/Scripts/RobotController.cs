using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace StarterAssets
{
    public class RobotController : NetworkBehaviour
    {
        //networked movement
        [SerializeField] private float m_MovementSpeed = 3.0f;
        
        private Vector3 mouse_pos;
        private Vector3 object_pos;
        private float angle;

        public NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> gameStarted = new NetworkVariable<bool>(false);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            isAlive.OnValueChanged += Die;
            //set the player's color
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = Random.ColorHSV();
        }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            if (!IsOwner)
            {
                return;
            }
            
            Move();

            LookAtMouse();
        }

        private void Move()
        {
            var movement = new Vector3(
                Keyboard.current.aKey.isPressed ? -1 : Keyboard.current.dKey.isPressed ? 1 : 0,
                0,
                Keyboard.current.sKey.isPressed ? -1 : Keyboard.current.wKey.isPressed ? 1 : 0
            );
            if (movement is { z: 0, x: 0 })
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                //move with rigidbody
                GetComponent<Rigidbody>().velocity = movement * m_MovementSpeed;
            }
        }

        private void LookAtMouse()
        {
            mouse_pos = Mouse.current.position.value;
            mouse_pos.z = 5.23f; //The distance between the camera and object
            object_pos = Camera.main.WorldToScreenPoint(transform.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
        }

      

        public void Dash(float velocity)
        {
            if(gameStarted.Value == false)
                return;
            GetComponent<Rigidbody>().velocity = transform.right * velocity;
        }

        public void Attack()
        {
            if(gameStarted.Value == false)
                return;
            // if left click is hit
            Debug.Log("left click");
            if (Physics.OverlapSphere(new Vector3(transform.right.x + transform.position.x, transform.position.y + transform.right.y, transform.position.z + transform.right.z), 3f, LayerMask.GetMask("Ball")).Length > 0)
            {
                Debug.Log(LayerMask.GetMask("Ball"));
                Physics.OverlapSphere(transform.position, 10f, LayerMask.GetMask("Ball"))[0].gameObject
                    .GetComponent<BallController>().recieveHit(new Vector3(transform.forward.z, transform.forward.y, -transform.forward.x));


                Debug.Log("hit");

            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(transform.right.x + transform.position.x , transform.position.y + transform.right.y, transform.position.z + transform.right.z), 3f);
        }
        
        public void Die(bool oldVal, bool newVal)
        {
            if (!IsOwner || !IsServer)
                return;
            
            if (newVal == false)
                Destroy(gameObject);
        }
    }
}