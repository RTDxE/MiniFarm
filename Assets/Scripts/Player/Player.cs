using UnityEngine;

namespace MiniFarm.Player
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private PlayerMove _playerMove;
        public PlayerMove PlayerMove => _playerMove;
        
        [SerializeField] private PlayerInteract _playerInteract;
        public PlayerInteract PlayerInteract => _playerInteract;
        
        [SerializeField] private PlayerInventory _playerInventory;
        public PlayerInventory PlayerInventory => _playerInventory;
        
    }
}