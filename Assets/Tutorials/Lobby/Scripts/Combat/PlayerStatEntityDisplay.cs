using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lobbytest.Mirror.Tutorials.Lobby;
//using System;
//using System.Linq;
using TMPro;
using Mirror;
using System;
using Lobbytest.Mirror.Tutorials.Lobby.Networking;

namespace Lobbytest.Mirror.Tutorials.Lobby.Combat
{
    public class PlayerStatEntityDisplay : MonoBehaviour
    {
        [SerializeField] private Image characterIconImage = null;
        [SerializeField] private TMP_Text playerNameText = null;
        [SerializeField] private TMP_Text scoreText = null;
        [SerializeField] private Image healthBarImage = null;

        public uint PlayerNetId { get; private set; }

        public void SetUp(Player player)
        {
            // Set up the display
            PlayerNetId = player.netId;
            NetworkGamePlayerLobby gamePlayer = NetworkServer.spawned[player.OwnerId].GetComponent<NetworkGamePlayerLobby>();
            playerNameText.text = gamePlayer.DisplayName;
            scoreText.text = gamePlayer.Score.ToString();
            if (!player.TryGetComponent<Health>(out var health))
            {
                return;
            }
            health.OnHealthChanged += HandleHealthChanged;
        }

       

        private void HandleHealthChanged(object sender, HealthChangedEventArgs e)
        {
            healthBarImage.fillAmount = e.Health / e.MaxHealth;
        }

        //public void FirstOrDefault()
        //{

        //}
    }
}
