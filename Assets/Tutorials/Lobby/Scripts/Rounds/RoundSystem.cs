using Lobbytest.Mirror.Tutorials.Lobby.Combat;
using Lobbytest.Tutorials.Lobby;
using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Lobbytest.Mirror.Tutorials.Lobby.Networking;
namespace Lobbytest.Mirror.Tutorials.Lobby.Rounds
{
    public class RoundSystem : NetworkBehaviour
    {
        private List<Player> remainingPlayers { get; } = new List<Player>();
        [SerializeField] private Animator animator = null;

        private NetworkManagerLobby room;


        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }
        public void CountdownEnded()
        {
            animator.enabled = false;
        }
        public override void OnStartServer()
        {
            NetworkManagerLobby.OnServerStopped += CleanUpServer;
            NetworkManagerLobby.OnServerReadied += CheckToStartRound;
        }
        [ServerCallback]
        private void OnDestroy() => CleanUpServer();
        [Server]
        private void CleanUpServer()
        {
            NetworkManagerLobby.OnServerStopped -= CleanUpServer;
            NetworkManagerLobby.OnServerReadied -= CheckToStartRound;
        }
        [ServerCallback]
        public void StartRound()
        {
            RpcStartRound();
        }

        [Server]
        private void CheckToStartRound(NetworkConnection conn)
        {
            if (Room.GamePlayers.Count(x => x.connectionToClient.isReady) != Room.GamePlayers.Count) { return; }

            animator.enabled = true;

            RpcStartCountdown();
        }

        [ClientRpc]
        private void RpcStartCountdown()
        {
            animator.enabled = true;
        }
        [ClientRpc]
        private void RpcStartRound()
        {
            InputManager.Remove(ActionMapNames.Player);
        }
        [Server]
        private void HandleDeath(object sender, DeathEventArgs e)
        {
            if (remainingPlayers.Count == 1)
            {
                return;
            }
            foreach (var player in remainingPlayers)
            {
                if (player == null || player.connectionToClient == e.ConnectionToClient)
                {
                    remainingPlayers.Remove(player);
                    break;
                }
            }
            if (remainingPlayers.Count != 1)
            {
                return;
            }
            HandleRoundEnd();

        }
        [Server]
        private void HandleRoundEnd()
        {
            remainingPlayers[0].IncrementScore();
            Room.GoToNextMap();
        }
    }
}
