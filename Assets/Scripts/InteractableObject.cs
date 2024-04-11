using UnityEngine;
using UnityEngine.Events;

namespace MiniFarm
{
    public class InteractableObject : MonoBehaviour
    {
        public UnityEvent<Player> onEnter;
        public UnityEvent<Player> onExit;
        public UnityEvent<Player> onInteract;
        
        public void Enter(Player player)
        {
            onEnter.Invoke(player);
        }
        
        public void Exit(Player player)
        {
            onExit.Invoke(player);
        }
        
        public void Interact(Player player)
        {
            onInteract.Invoke(player);
        }
    }
}