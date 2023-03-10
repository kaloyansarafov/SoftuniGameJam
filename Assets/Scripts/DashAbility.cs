using UnityEngine;

namespace StarterAssets
{
    [CreateAssetMenu]
    public class DashAbility : Ability
    {
        public float velocity;
        
        public override void Use(GameObject player)
        {
            base.Use(player);
            RobotController controller = player.GetComponent<RobotController>();
            controller.Dash(velocity);
        }
    }
}