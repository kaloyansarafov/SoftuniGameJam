using UnityEngine;

namespace StarterAssets
{
    public class Ability : ScriptableObject
    {
        public new string name;
        public float cooldown;
        public float duration;
        
        public virtual void Use(GameObject player)
        {
            Debug.Log("Using ability: " + name);
        }
    }
}