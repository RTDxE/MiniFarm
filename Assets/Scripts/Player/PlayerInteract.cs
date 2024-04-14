using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniFarm
{
    [RequireComponent(typeof(Player.Player))]
    public class PlayerInteract : MonoBehaviour
    {
        private Player.Player _player;
        private InteractableObject _lastObject;

        private bool inShopArea;
        [SerializeField] private InteractableObject _shop;

        [SerializeField] private UnityEvent onEnterInteract;
        [SerializeField] private UnityEvent onExitInteract;

        private void Start()
        {
            _player = GetComponent<Player.Player>();
        }

        private void Update()
        {
            if (inShopArea)
            {
                if (_lastObject != _shop)
                    _lastObject = _shop;
                return;
            }
            var gObject = ObjectsManager.Instance.GetObject(transform.position);
            InteractableObject obj;
            if (gObject == null || !gObject.TryGetComponent(out obj))
            {
                if (_lastObject == null) return;
                _lastObject.Exit(_player);
                _lastObject = null;
                onExitInteract.Invoke();
                return;
            }

            if (obj != _lastObject && _lastObject != null)
            {
                _lastObject.Exit(_player);
            }

            if (_lastObject == null)
            {
                onEnterInteract.Invoke();
            }

            _lastObject = obj;
            _lastObject.Enter(_player);
        }

        public void Interact()
        {
            _lastObject.Interact(_player);
        }

        private void OnTriggerEnter(Collider other)
        {
            inShopArea = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _lastObject = null;
            inShopArea = false;
        }
    }

    public class MyItem
    {
        public string name;
    }

    public class MyToolItem : MyItem
    {
        public int durability;
    }
    
    public class MyNewToolItem : MyToolItem
    {
        public bool isNew;
    }

    public class RandomClass
    {
        public void SomeDo(MyItem item) // item -> MyToolItem
        {
            // if (item.GetType() == typeof(MyToolItem)) {}
            
            // (item as MyToolItem).durability;
            // ((MyToolItem)item).durability;

            // if (item is MyToolItem toolItem)
            // {
            //     item.name;
            //     toolItem.durability;
            // }
        }

        public void NoMore(MyNewToolItem item)
        {
            // item type == MyItem
            // typeof(MyItem).IsInstanceOfType(item)
        }
    }
}