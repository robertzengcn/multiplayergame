using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Lobbytest.Tutorials.Lobby;
namespace Lobbytest.Mirror.Tutorials.Lobby
{
    public class NetworkManagerLobby : NetworkManager
    {
        [SerializeField] private int minPlayers = 2;
        [SerializeField] private int maxPlayers = 2;
        [Scene][SerializeField] private string menuScene = string.Empty;

        [Header("Maps")]
        [SerializeField] private int numberOfRounds = 1;
        [SerializeField] private MapSet mapSet = null;

        [Header("Room")]
        [SerializeField] private NetworkRoomPlayerLobby roomPlayerPrefab = null;


        [Header("Game")]
        [SerializeField] private NetworkGamePlayerLobby gamePlayerPrefab = null;
        [SerializeField] private GameObject playerSpawnSystem = null;
        [SerializeField] private GameObject roundSystem = null;
        private MapHandler mapHandler;

        public static event Action OnClientConnected;
        public static event Action OnClientDisconnected;
        public static event Action<NetworkConnection> OnServerReadied;
        public static event Action OnServerStopped;

        public List<NetworkRoomPlayerLobby> RoomPlayers { get; } = new List<NetworkRoomPlayerLobby>();
        public List<NetworkGamePlayerLobby> GamePlayers { get; } = new List<NetworkGamePlayerLobby>();

        public override void OnStartServer() => spawnPrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs").ToList();

        public override void OnStartClient()
        {
            Debug.Log("start client");
            var spawnablePrefabs = Resources.LoadAll<GameObject>("SpawnablePrefabs");

            foreach (var prefab in spawnablePrefabs)
            {
                NetworkClient.RegisterPrefab(prefab);
            }
        }
        public override void OnClientConnect()
        {
            base.OnClientConnect();

            OnClientConnected?.Invoke();
        }
        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();

            OnClientDisconnected?.Invoke();
        }
        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            Debug.Log("numPlayers is" + numPlayers);
            Debug.Log("max connect is" + maxConnections);
            if (numPlayers >= maxConnections)
            {
                conn.Disconnect();
                return;
            }
            Debug.Log("scene path " + SceneManager.GetActiveScene().path);
            Debug.Log("menu scene path " + menuScene);
            if (SceneManager.GetActiveScene().path != menuScene)
            {
                conn.Disconnect();
                return;
            }

        }
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Debug.Log("on server app player");
            //Debug.Log("active scene name is " + SceneManager.GetActiveScene().name);
            //Debug.Log("menuScene name is" + menuScene);
            if (SceneManager.GetActiveScene().name == "Scene_Lobby")
            {
                Debug.Log("on server app player in menu scene");
                bool isLeader = RoomPlayers.Count == 0;

                NetworkRoomPlayerLobby roomPlayerInstance = Instantiate(roomPlayerPrefab);

                roomPlayerInstance.IsLeader = isLeader;

                NetworkServer.AddPlayerForConnection(conn, roomPlayerInstance.gameObject);
            }
        }
        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (conn.identity != null)
            {
                var player = conn.identity.GetComponent<NetworkRoomPlayerLobby>();

                RoomPlayers.Remove(player);

                NotifyPlayersOfReadyState();
            }

            base.OnServerDisconnect(conn);
        }
        public override void OnStopServer()
        {
            OnServerStopped?.Invoke();

            RoomPlayers.Clear();
            GamePlayers.Clear();
        }
        public void NotifyPlayersOfReadyState()
        {
            foreach (var player in RoomPlayers)
            {
                player.HandleReadyToStart(IsReadyToStart());
            }
        }
        private bool IsReadyToStart()
        {
            if (numPlayers < minPlayers) { return false; }

            foreach (var player in RoomPlayers)
            {
                if (!player.IsReady) { return false; }
            }

            return true;
        }
        public void StartGame()
        {
            Debug.Log("start gmae");
            Debug.Log("scene path is " + SceneManager.GetActiveScene().path);
            Debug.Log("menu scene is " + menuScene);
            if (SceneManager.GetActiveScene().path == menuScene)
            {
                if (!IsReadyToStart()) { return; }

                mapHandler = new MapHandler(mapSet, numberOfRounds);

                ServerChangeScene(mapHandler.NextMap);
                ServerChangeScene("Scene_Map_01");
            }
        }
        public override void ServerChangeScene(string newSceneName)
        {
            Debug.Log(SceneManager.GetActiveScene().path);
            Debug.Log(menuScene);
            // From menu to game
            if (SceneManager.GetActiveScene().path == menuScene && newSceneName.StartsWith("Scene_Map"))
            {
                Debug.Log("server change scene");
                for (int i = RoomPlayers.Count - 1; i >= 0; i--)
                {
                    var conn = RoomPlayers[i].connectionToClient;
                    var gameplayerInstance = Instantiate(gamePlayerPrefab);
                    gameplayerInstance.SetDisplayName(RoomPlayers[i].DisplayName);

                    //NetworkServer.Destroy(conn.identity.gameObject);
                    //GameObject destoryone = conn.identity.gameObject;
                    NetworkServer.ReplacePlayerForConnection(conn, gameplayerInstance.gameObject);
                    //NetworkServer.Destroy(destoryone);
                }
            }

            base.ServerChangeScene(newSceneName);
        }

        public override void OnServerSceneChanged(string sceneName)
        {
            if (sceneName.StartsWith("Scene_Map"))
            {
                GameObject playerSpawnSystemInstance = Instantiate(playerSpawnSystem);
                NetworkServer.Spawn(playerSpawnSystemInstance);

                //GameObject roundSystemInstance = Instantiate(roundSystem);
                //NetworkServer.Spawn(roundSystemInstance);
            }
        }
        public override void OnServerReady(NetworkConnectionToClient conn)
        {
            base.OnServerReady(conn);

            OnServerReadied?.Invoke(conn);
        }



    }
}
