using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
namespace Lobbytest.Mirror.Tutorials.Lobby.Networking
{
    public class Player : NetworkBehaviour
    {
        public static event Action<Player> OnPlayerSpawned;
        public static event Action<Player> OnPlayerDespawned;

        [SyncVar(hook = nameof(HandleOwnerSet))]
        private uint ownerId;

        public uint OwnerId => ownerId;
        private void OnDestroy()
        {
            OnPlayerDespawned?.Invoke(this);
        }
        private void HandleOwnerSet(uint oldOwnerId, uint newOwnerId)
        {
            OnPlayerSpawned?.Invoke(this);
        }

        internal void IncrementScore()
        {
            throw new NotImplementedException();
        }
    }
}
