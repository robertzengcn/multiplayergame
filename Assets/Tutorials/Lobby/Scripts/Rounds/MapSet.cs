using System.Collections.Generic;
using UnityEngine;
using Mirror;
namespace Lobbytest.Tutorials.Lobby.Rounds
{
    [CreateAssetMenu(fileName = "New Map Set", menuName = "Rounds/Map Set")]
    public class MapSet : ScriptableObject
    {
        [Scene]
        [SerializeField] private List<string> maps = new List<string>();
        public IReadOnlyCollection<string> Maps => maps.AsReadOnly();
    }
}
