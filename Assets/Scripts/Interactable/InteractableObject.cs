using UnityEngine;
using UnityEngine.Events;

namespace MiniFarm
{
    public class InteractableObject : MonoBehaviour
    {
        public UnityEvent<Player.Player> onEnter;
        public UnityEvent<Player.Player> onExit;
        public UnityEvent<Player.Player> onInteract;
        
        public void Enter(Player.Player player)
        {
            onEnter.Invoke(player);
        }
        
        public void Exit(Player.Player player)
        {
            onExit.Invoke(player);
        }
        
        public void Interact(Player.Player player)
        {
            onInteract.Invoke(player);
        }
    }
}