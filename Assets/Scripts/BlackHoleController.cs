using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace StarterAssets
{
    public class BlackHoleController : NetworkBehaviour
    {
        [SerializeField] private float m_MovementSpeed = 3.0f;
        [SerializeField] private float m_Radius = 3.0f;
    }
}