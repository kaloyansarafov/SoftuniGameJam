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

        public Animator animator;
        public override void OnNetworkSpawn()
        {
            
            base.OnNetworkSpawn();
            isAlive.OnValueChanged += Die;
            //set the player's color
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = Random.ColorHSV();
            animator.enabled = false;
            animator.enabled = true;
        }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            GetComponentInChildren<Animator>().SetBool("Test", true);
            animator.SetBool("Test", true);
            if (!IsOwner)
            {
                return;
            }
            
            Move();

            LookAtMouse();
        }

        private void Move()
        {
            if (Keyboard.current.wKey.isPressed)
            {
                gameObject.GetComponentInChildren<Animator>().SetInteger("Movement", 1);
                var movement = transform.right * m_MovementSpeed * Time.deltaTime;
                GetComponent<Rigidbody>().MovePosition(transform.position + movement);
            }
            else
            {
                gameObject.GetComponentInChildren<Animator>().SetInteger("Movement", 0);
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
            animator.SetTrigger("Attack");
            // if left click is hit
            Debug.Log("left click");
            GetComponentInChildren<Animator>().SetTrigger("Attack");
            if (Physics.OverlapSphere(new Vector3(transform.right.x + transform.position.x, transform.position.y + transform.right.y, transform.position.z + transform.right.z), 2f, LayerMask.GetMask("Ball")).Length > 0)
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