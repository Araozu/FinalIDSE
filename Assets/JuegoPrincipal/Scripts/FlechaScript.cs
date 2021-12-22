using UnityEngine;
using UnityEngine.Serialization;

namespace JuegoPrincipal.Scripts
{
    public class FlechaScript : MonoBehaviour
    {
        private Vector3 _position;
        public float rotationSpeed = 10;

        //values for internal use
        private Quaternion _lookRotation;
        private Vector3 _direction;

        // Update is called once per frame
        private void Update()
        {
            Vector2 dir = _position - transform.position;
            transform.right = dir;
        }

        public void SetTarget(Vector3 newPosition)
        {
            _position = newPosition;
        }
    }
}