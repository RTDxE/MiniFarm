using MiniFarm.Items;
using MiniFarm.ScriptableObjects;
using MiniFarm.UI;
using UnityEngine;
using UnityEngine.Events;

namespace MiniFarm
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        public InventoryData Inventory => _inventory;

        [SerializeField] private Joystick _joystick;
        [SerializeField] private Rigidbody2D _rigidbody;

        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float smoothSpeed = 1f;

        private readonly int _activeItemIdx = 0;
        public ItemInstance ActiveItem => _inventory.items[_activeItemIdx];

        private InteractableObject _lastObject;

        [SerializeField] private UnityEvent onEnterInteract;
        [SerializeField] private UnityEvent onExitInteract;
        
        // TODO только для теста. УДОЛИТЬ
        [SerializeField] private GameObject garden;
        
        private void Start()
        {
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

        public ItemInstance TakeActiveItem(int count = 1)
        {
            return Inventory.Take(ActiveItem.item, count);
        }

        public void GiveItem(ItemInstance item)
        {
            Inventory.Give(item);
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, _joystick.Direction * moveSpeed, Time.fixedDeltaTime / smoothSpeed);
        }

        private void Update()
        {
            var gObject = ObjectsManager.Instance.GetObject(transform.position);
            InteractableObject obj;
            if (gObject == null || !gObject.TryGetComponent(out obj))
            {
                if (_lastObject == null) return;
                _lastObject.Exit(this);
                _lastObject = null;
                onExitInteract.Invoke();
                return;
            }
            
            if (obj != _lastObject && _lastObject != null)
            {
                _lastObject.Exit(this);
            }

            if (_lastObject == null)
            {
                onEnterInteract.Invoke();
            }
            
            _lastObject = obj;
            _lastObject.Enter(this);
        }

        public void Interact()
        {
            _lastObject.Interact(this);
        }
    }
}