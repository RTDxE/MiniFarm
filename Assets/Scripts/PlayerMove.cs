using MiniFarm.UI;
using UnityEngine;

namespace MiniFarm
{
    public class PlayerMove : MonoBehaviour
    {
        
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Rigidbody2D _rigidbody;
        
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float smoothSpeed = 1f;
        
        private void FixedUpdate()
        {
            _rigidbody.velocity = Vector2.Lerp(_rigidbody.velocity, _joystick.Direction * moveSpeed, Time.fixedDeltaTime / smoothSpeed);
        }
    }
}