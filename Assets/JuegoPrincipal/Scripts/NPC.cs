using System;
using Unity.Mathematics;
using UnityEngine;

namespace JuegoPrincipal.Scripts
{
    public enum Estado
    {
        Acelerando,
        Frenando,
        Reposo,
    }

    public class NPC : MonoBehaviour
    {
        private Estado _estado = Estado.Acelerando;

        private float driftFactor = 0.0f;
        private float acelerationFactor = 750f;
        private float turnFactor = 1.0f;

        private float acelerationInput = 1;
        private float steeringInput = 0;

        private float rotationAngle = 0;

        // Define con cuanta fuerza frenar.
        // Cambia segun la distancia con el objeto al frente.
        private float _fuerzaFreno = 0;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            switch (_estado)
            {
                case Estado.Acelerando when Velocidad() < 50:
                    Acelerar();
                    RemoveOrthogonalForces();
                    break;
                case Estado.Frenando:
                    Frenar1();
                    RemoveOrthogonalForces();
                    break;
                case Estado.Reposo:
                default:
                    _rb.velocity = Vector2.zero;
                    break;
            }
        }

        // Acelera o frena
        private void Acelerar()
        {
            // Fuerza del motor
            var fuerza = transform.up * acelerationFactor * Time.deltaTime;
            _rb.AddForce(fuerza, ForceMode2D.Force);
        }

        private void Frenar1()
        {
            // Fuerza del motor
            var fuerza = transform.up * -_fuerzaFreno * acelerationFactor * Time.deltaTime;
            _rb.AddForce(fuerza, ForceMode2D.Force);

            // Al terminar de frenar establecer el estado a reposo
            if (_rb.velocity.magnitude < 0.5)
            {
                Debug.Log("Reposo!!!");
                _estado = Estado.Reposo;
            }
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
        private float Velocidad()
        {
            var velocidadAdelante = transform.up * _rb.velocity;
            var esMovimientoHaciaAdelante = velocidadAdelante.x > 0 || velocidadAdelante.y > 0;
            var magnitudVelocidad = velocidadAdelante.magnitude * 2.5f;

            return esMovimientoHaciaAdelante ? magnitudVelocidad : -magnitudVelocidad;
        }

        // Distancia entre el vehiculo adelante y este vehiculo
        internal void Frenar(float distancia)
        {
            var velocidad = Velocidad() / 10;

            if (velocidad <= 0.1)
            {
                Debug.Log("Velocidad minima");
                _rb.velocity = Vector2.zero;
                _estado = Estado.Reposo;
                return;
            }

            var fuerzaFreno = 6 - distancia;
            if (fuerzaFreno < 0)
            {
                fuerzaFreno = 0;
            }

            // Sumar la velocidad actual / 10, para que si va muy rapido frene mas
            fuerzaFreno *= (velocidad - 10) / 10;
            Debug.Log("Freno: " + fuerzaFreno);

            // TODO: Dejar de frenar segun la distancia

            _fuerzaFreno = math.abs(fuerzaFreno);
            _estado = Estado.Frenando;
        }
    }
}