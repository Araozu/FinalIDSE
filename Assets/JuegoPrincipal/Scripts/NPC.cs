using System;
using UnityEngine;

namespace JuegoPrincipal.Scripts
{
    public class NPC : MonoBehaviour
    {
        private float driftFactor = 0.1f;
        private float acelerationFactor = 750f;
        private float turnFactor = 1.0f;

        private float acelerationInput = 1;
        private float steeringInput = 0;

        private float rotationAngle = 0;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            Acelerar();
            RemoveOrthogonalForces();
        }

        // Acelera o frena
        private void Acelerar()
        {
            var velocity = GetVelocity();
            if (velocity > 50) return;

            if (acelerationInput <= 0 && velocity < 0.1)
            {
                _rb.velocity = Vector2.zero;
                return;
            }

            // Hacer que el frenado sea mas fuerte
            var nuevaAceleracion = acelerationInput < 0 ? acelerationInput * 3 : acelerationInput;

            // Fuerza del motor
            var fuerza = transform.up * nuevaAceleracion * acelerationFactor * Time.deltaTime;
            _rb.AddForce(fuerza, ForceMode2D.Force);
        }

        private void RemoveOrthogonalForces()
        {
            var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
            var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

            _rb.velocity = forwardVelocity + rightVelocity * driftFactor;
        }

        /**
         * Devuelve la velocidad del vehiculo en km/h, positivo si va hacia adalente,
         * negativo si va hacia atras
         */
        private float GetVelocity()
        {
            var velocidadAdelante = transform.up * _rb.velocity;
            var esMovimientoHaciaAdelante = velocidadAdelante.x > 0 || velocidadAdelante.y > 0;
            var magnitudVelocidad = velocidadAdelante.magnitude * 2.5f;

            return esMovimientoHaciaAdelante ? magnitudVelocidad : -magnitudVelocidad;
        }

        internal void Frenar()
        {
            if (acelerationInput >= -1)
            {
                acelerationInput = -1;
            }
        }
    }
}