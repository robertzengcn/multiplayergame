using Mirror;
using UnityEngine;
using System;

namespace Lobbytest.Mirror.Tutorials.Lobby.Combat
{
    public class Health : NetworkBehaviour
    {
        [Header("Setting")]
        [SerializeField]private float maxHealth = 100f;
        [SyncVar(hook = nameof(HandleHealthChanged))]
        private float health = 0f;

        public static event EventHandler<DeathEventArgs> OnDeath;
        public event EventHandler<HealthChangedEventArgs> OnHealthChanged;

        public bool IsDead=> health == 0f;
        public override void OnStartServer()
        {
            health = maxHealth;
        }
        [ServerCallback]
        public void OnDestory(float value)
        {
            OnDeath?.Invoke(this,new DeathEventArgs { ConnectionToClient = connectionToClient });
        }
        [Server]
        public void Add(float value)
        {
            value=Mathf.Max(value, 0f );
            health = Mathf.Min(health + value, maxHealth);
        }
        [Server]
        public void Remove(float value)
        {
           
            value = Mathf.Max(value, 0f);
            health = Mathf.Max(health - value, 0f);
            if (health == 0f)
            {
                OnDeath?.Invoke(this,new DeathEventArgs { ConnectionToClient = connectionToClient });
                RpcHandleDeath();
            }
        }
        private void HandleHealthChanged(float oldValue,float newValue)
        {
            OnHealthChanged?.Invoke(this, new HealthChangedEventArgs
            {
                Health=health,
                MaxHealth = maxHealth
            });
        }
        [ClientRpc]
        private void RpcHandleDeath()
        {
           gameObject.SetActive(false);
        }


    }
}
