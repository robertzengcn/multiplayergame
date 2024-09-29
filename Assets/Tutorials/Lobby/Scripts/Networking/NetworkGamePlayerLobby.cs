using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lobbytest.Mirror.Tutorials.Lobby.Networking
{

    public class NetworkGamePlayerLobby : NetworkBehaviour
    {

        [SyncVar]
        private string displayName = "Loading...";

        public string DisplayName
        {
            get
            {
                return displayName;
            }

           
        }

        private int score;
        public int Score
        {
            get
            {
                return score;
            }

        }
        private NetworkManagerLobby room;

        private NetworkManagerLobby Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as NetworkManagerLobby;
            }
        }

        public override void OnStartClient()
        {
            DontDestroyOnLoad(gameObject);

            Room.GamePlayers.Add(this);
        }

        public override void OnStopClient()
        {
            Room.GamePlayers.Remove(this);
        }


        [Server]
        public void SetDisplayName(string displayName)
        {
            this.displayName = displayName;
        }



    }
}
