using Mirror;
using System;

namespace Lobbytest.Mirror.Tutorials.Lobby.Combat
{
    public class DeathEventArgs : EventArgs
    {
        public NetworkConnection ConnectionToClient { get; set; }
    }
}