using UnityEngine;

namespace StarterAssets
{
    [CreateAssetMenu]
    public class MainAttackAbility : Ability
    {
        
        public override void Use(GameObject player)
        {
            base.Use(player);
            RobotController controller = player.GetComponent<RobotController>();
            controller.Attack();
        }
    }
}