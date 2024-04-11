using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniFarm.UI
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private Vector2 _direction = Vector2.zero;
        public Vector2 Direction => _direction;

        [SerializeField] private GameObject _upButton;
        [SerializeField] private GameObject _downButton;
        [SerializeField] private GameObject _leftButton;
        [SerializeField] private GameObject _rightButton;

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateDirection(eventData.pointerCurrentRaycast.gameObject);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UpdateDirection(null);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateDirection(eventData.pointerCurrentRaycast.gameObject);
        }

        private void UpdateDirection(GameObject currentButton)
        {
            if (currentButton == null)
            {
                _direction = Vector2.zero;
                return;
            }

            if (currentButton.Equals(_upButton))
            {
                _direction = Vector2.up;
            }
            else if (currentButton.Equals(_downButton))
            {
                _direction = Vector2.down;
            }
            else if (currentButton.Equals(_leftButton))
            {
                _direction = Vector2.left;
            }
            else if (currentButton.Equals(_rightButton))
            {
                _direction = Vector2.right;
            }
        }
    }
}
