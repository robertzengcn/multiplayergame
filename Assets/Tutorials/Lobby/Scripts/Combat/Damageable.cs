using Mirror;
using UnityEngine;
namespace Lobbytest.Mirror.Tutorials.Lobby.Combat
{
    public class Damageable : NetworkBehaviour
    {
        [SerializeField] private Health health = null;
        public void DealDamage(float damageToDeal)
        {
            health.Remove(damageToDeal);
        }
    }
}
