using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace StarterAssets
{
    public class PlayerAbilityController : NetworkBehaviour
    {
        public Ability ability;
        public Key abilityKey = Key.F;
        private float cooldown;
        private float duration;
        
        enum AbilityState
        {
            Ready,
            Active,
            Cooldown
        }
        AbilityState state = AbilityState.Ready;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            //set the player's color
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            meshRenderer.material.color = Random.ColorHSV();
        }
        
        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            switch (state)
            {
                case AbilityState.Ready:
                    if (Keyboard.current[Key.F].wasPressedThisFrame)
                    {
                        ability.Use(gameObject);
                        state = AbilityState.Active;
                        duration = ability.duration;
                    }
                    break;
                case AbilityState.Active:
                    if (duration > 0)
                    {
                        duration -= Time.deltaTime;
                    }
                    else
                    {
                        state = AbilityState.Cooldown;
                        cooldown = ability.cooldown;
                    }
                    
                    break;
                case AbilityState.Cooldown:
                    if (cooldown > 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        state = AbilityState.Ready;
                    }
                    break;
                default:
                    break;
            }
            
            
        }
    }
}