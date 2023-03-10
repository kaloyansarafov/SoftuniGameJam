using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class RobotController : NetworkBehaviour
    {
        //networked movement
        [SerializeField] private float m_MovementSpeed = 3.0f;
        
        private Vector3 mouse_pos;
        private Vector3 object_pos;
        private float angle;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
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
            
            MeleeAttack();
        }

        private void Move()
        {
            var movement = new Vector3(
                Keyboard.current.aKey.isPressed ? -1 : Keyboard.current.dKey.isPressed ? 1 : 0,
                0,
                Keyboard.current.sKey.isPressed ? -1 : Keyboard.current.wKey.isPressed ? 1 : 0
            );
            transform.position += movement * (m_MovementSpeed * Time.deltaTime);
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

        private void MeleeAttack()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out var hit))
                {
                    
                }
            }
        }
    }
}