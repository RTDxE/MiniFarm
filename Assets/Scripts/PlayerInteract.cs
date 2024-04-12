using UnityEngine;
using UnityEngine.Events;

namespace MiniFarm
{
    [RequireComponent(typeof(Player))]
    public class PlayerInteract : MonoBehaviour
    {
        private Player _player;
        private InteractableObject _lastObject;

        [SerializeField] private UnityEvent onEnterInteract;
        [SerializeField] private UnityEvent onExitInteract;

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
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
    }
}