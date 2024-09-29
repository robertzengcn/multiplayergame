using Mirror;
using System;


namespace Lobbytest.Mirror.Tutorials.Lobby.Combat
{
    public class HealthChangedEventArgs : EventArgs
    {
        public  float Health { get; set; }
        public float MaxHealth { get; set; }
    }
}