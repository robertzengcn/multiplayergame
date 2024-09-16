using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lobbytest.Mirror.Tutorials.Lobby
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby networkManager = null;
        [Header("UI")]
        [SerializeField] private GameObject landingPagePanel = null;

        // Start is called before the first frame update
        public void HostLobby()
        {
            networkManager.StartHost();

            landingPagePanel.SetActive(false);
        }
    }
}
