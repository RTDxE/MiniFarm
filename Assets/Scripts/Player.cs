using UnityEngine;

namespace MiniFarm
{
    public class Player: MonoBehaviour
    {
        // TODO только для теста. УДОЛИТЬ
        [SerializeField] private GameObject garden;

        [SerializeField] private PlayerMove _playerMove;
        public PlayerMove PlayerMove => _playerMove;
        
        [SerializeField] private PlayerInteract _playerInteract;
        public PlayerInteract PlayerInteract => _playerInteract;
        
        [SerializeField] private PlayerInventory _playerInventory;
        public PlayerInventory PlayerInventory => _playerInventory;
        
        private void Start()
        {
            // TODO только для теста. УДОЛИТЬ
            {
                var gardenInst = Instantiate(garden);
                ObjectsManager.Instance.PlaceObject(gardenInst);
            }
            {
                var gardenInst = Instantiate(garden);
                gardenInst.transform.position = new Vector3(1.4f, 0, 0);
                ObjectsManager.Instance.PlaceObject(gardenInst);
            }
            {
                var gardenInst = Instantiate(garden);
                gardenInst.transform.position = new Vector3(1.4f, 1.1f, 0);
                ObjectsManager.Instance.PlaceObject(gardenInst);
            }
        }
    }
}