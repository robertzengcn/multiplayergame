using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lobbytest.Mirror.Tutorials.Lobby.Networking;
using System.Linq;
namespace Lobbytest.Mirror.Tutorials.Lobby.Combat
{
    public class PlayerStatsDisplay : MonoBehaviour
    {
        [SerializeField] private PlayerStatEntityDisplay statEntityDisplay = null;
        [SerializeField] private Transform statEntityHolderTransform = null;

        private readonly List<PlayerStatEntityDisplay> statEntityDisplays = new List<PlayerStatEntityDisplay>();

        private void Awake()
        {
            Player.OnPlayerSpawned += HandlePlayerSpawned;
            Player.OnPlayerDespawned += HandlePlayerDespawned;
        }
        public static event Action<Player> OnPlayerSpawned;
        public static event Action<Player> OnPlayerDespawned;

        private void OnDestroy()
        {
            Player.OnPlayerSpawned -= HandlePlayerSpawned;
            Player.OnPlayerDespawned -= HandlePlayerDespawned;
        }

        private void HandlePlayerSpawned(Player player)
        {
            PlayerStatEntityDisplay displayInstance = Instantiate(statEntityDisplay, statEntityHolderTransform);
            displayInstance.SetUp(player);
            statEntityDisplays.Add(displayInstance);

        }

        private void HandlePlayerDespawned(Player player)
        {
            PlayerStatEntityDisplay displayInstance = statEntityDisplays.FirstOrDefault(x => x.PlayerNetId == player.netId);
            if (displayInstance == null)
            {
                return;
            }

            statEntityDisplays.Remove(displayInstance);
            Destroy(displayInstance.gameObject);

        }
    }

}
