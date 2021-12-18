using System;
using System.Collections;
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

        private const float AcelerationFactor = 750f;
        private const int MaximaVelocidad = 30;
        private float steeringInput = 0;

        private bool _desactivarMovimiento = false;

        // Define con cuanta fuerza frenar.
        // Cambia segun la distancia con el objeto al frente.
        private float _fuerzaFreno;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_desactivarMovimiento) return;

            switch (_estado)
            {
                case Estado.Acelerando when Velocidad() < MaximaVelocidad:
                    Acelerar();
                    RemoveOrthogonalForces();
                    break;
                case Estado.Frenando:
                    Frenar();
                    RemoveOrthogonalForces();
                    break;
                case Estado.Reposo:
                    _rb.velocity = Vector2.zero;
                    break;
            }
        }

        private void Acelerar()
        {
            var fuerza = 0.5f * transform.up * AcelerationFactor * Time.deltaTime;
            _rb.AddForce(fuerza, ForceMode2D.Force);
        }

        private void Frenar()
        {
            var fuerza = transform.up * -_fuerzaFreno * AcelerationFactor * Time.deltaTime;
            _rb.AddForce(fuerza, ForceMode2D.Force);

            // Al terminar de frenar establecer el estado a reposo
            if (_rb.velocity.magnitude < 0.5)
            {
                _estado = Estado.Reposo;
            }
        }

        private void RemoveOrthogonalForces()
        {
            var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
            var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

            // forward + right * driftFactor
            _rb.velocity = forwardVelocity + rightVelocity * 0.1f;
        }

        /**
         * Devuelve la velocidad del vehiculo en km/h, positivo si va hacia adalente,
         * negativo si va hacia atras
         */
        private float Velocidad()
        {
            var velocidadAdelante = transform.up * _rb.velocity;
            var esMovimientoHaciaAdelante = velocidadAdelante.x > 0 || velocidadAdelante.y > 0;
            var magnitudVelocidad = velocidadAdelante.magnitude * 4f;

            return esMovimientoHaciaAdelante ? magnitudVelocidad : -magnitudVelocidad;
        }

        /**
         * Informa al npc de la distancia entre este y otro vehiculo
         * distancia: valor entre 5 y -1. -1 Indica que no hay ningun vehiculo cerca
         */
        internal void SetDistanciaColision(float distancia)
        {
            if (distancia <= 0)
            {
                _estado = Estado.Acelerando;
                return;
            }

            // TODO: Actualizar el estado aqui dependiendo de la colision
            var velocidad = Velocidad() / 10;

            if (velocidad <= 0.1)
            {
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

            // TODO: Dejar de frenar segun la distancia

            _fuerzaFreno = math.abs(fuerzaFreno);
            _estado = Estado.Frenando;
        }

        private IEnumerator Desaparecer()
        {
            yield return new WaitForSeconds(3);
            var component = GetComponent<SpriteRenderer>();

            component.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.3f);
            component.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(0.3f);

            component.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.3f);
            component.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(0.3f);
            
            component.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.3f);
            component.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(0.3f);

            GetComponentInChildren<NPCDetector>().Destruir();
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D()
        {
            _rb.velocity = Vector2.zero;
            _desactivarMovimiento = true;
            StartCoroutine(Desaparecer());
        }
    }
}