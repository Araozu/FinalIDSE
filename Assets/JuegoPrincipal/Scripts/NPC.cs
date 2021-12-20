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

        private const float AcelerationFactor = 1000f;
        private const int MaximaVelocidad = 30;
        private float _rotationAngle;

        private bool _desactivarMovimiento = false;

        // Define con cuanta fuerza acelerar o frenar.
        // Cambia segun la distancia con el objeto al frente.
        private float _fuerzaMotor = 0.5f;

        // Indica hacia donde se dirige el vehiculo
        private Vector3 _puntoDestino = Vector3.zero;

        private Rigidbody2D _rb;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rotationAngle = _rb.rotation;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (_desactivarMovimiento) return;

            switch (_estado)
            {
                case Estado.Acelerando when Velocidad() < MaximaVelocidad:
                    AplicarFuerzaMotor();
                    // Acelerar();
                    RemoveOrthogonalForces();
                    break;
                case Estado.Frenando:
                    AplicarFuerzaMotor();
                    // Frenar();
                    RemoveOrthogonalForces();
                    break;
            }

            CalcularAnguloGiro();
        }

        private static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
        {
            Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
            float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
            return Vector2.Angle(vec1, vec2) * sign;
        }

        private void CalcularAnguloGiro()
        {
            var objetivo = _puntoDestino;
            if (objetivo == Vector3.zero) return;

            // Castear los vectores a Vector2 para eliminar la componente z,
            // y que no afecte el angulo de giro.
            var transform1 = transform;
            Vector2 direccionObjetivo = objetivo - transform1.position;
            Vector2 direccionActual = transform1.up;

            var anguloDiferencia = (float)Math.Round(AngleBetweenVector2(direccionActual, direccionObjetivo), 1);

            if (math.abs(anguloDiferencia) < 0.1) return;

            var cantidadRotacion = anguloDiferencia / 10;

            _rotationAngle += cantidadRotacion;
            _rotationAngle %= 360;
            _rb.MoveRotation(_rotationAngle);
        }

        private void AplicarFuerzaMotor()
        {
            var fuerza = transform.up * (_fuerzaMotor * AcelerationFactor * Time.deltaTime);
            _rb.AddForce(fuerza, ForceMode2D.Force);
        }

        private void RemoveOrthogonalForces()
        {
            var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
            var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

            // drifFactor = 0.9f  -- numero magico, otro valor hace que el npc gire mal
            // forward + right * driftFactor
            _rb.velocity = forwardVelocity + rightVelocity * 0.5f;
        }

        /**
         * Devuelve la velocidad del vehiculo en km/h, positivo si va hacia adalente,
         * negativo si va hacia atras
         */
        public float Velocidad()
        {
            var velocidadAdelante = transform.up * _rb.velocity;
            var esMovimientoHaciaAdelante = velocidadAdelante.x > 0 || velocidadAdelante.y > 0;
            var magnitudVelocidad = math.floor(velocidadAdelante.magnitude * 4f);

            return esMovimientoHaciaAdelante ? magnitudVelocidad : -magnitudVelocidad;
        }

        /**
         * Informa al npc de la distancia entre este y otro vehiculo, y calcula cuando deberia acelerar
         * distancia: valor positivo. Un valor mayor a 6 indica que no hay ningun vehiculo cerca
         * velocidadOther: Velocidad del vehiculo delante
         */
        internal void SetDistanciaColision(float distancia, float velocidadOther)
        {
            var velocidad = Velocidad();
            if (distancia >= 6)
            {
                _estado = Estado.Acelerando;
                _fuerzaMotor = 0.5f;
            }
            // Frenar o acelerar dependiendo de la velocidad del vehiculo adelante
            else if (distancia > 0.1f && distancia < 6)
            {
                var diferenciaVelocidad = velocidadOther - velocidad;

                // Si el vehiculo adelante va mas rapido que este, acelerar
                if (diferenciaVelocidad > 0)
                {
                    _estado = Estado.Acelerando;
                    _fuerzaMotor = 0.5f;
                    return;
                }

                // Si va a la misma velocidad, no hacer nada
                if (diferenciaVelocidad == 0)
                {
                    _estado = Estado.Reposo;
                    _fuerzaMotor = 0;
                    return;
                }

                // Sino frenar segun la distancia y velocidad
                _estado = Estado.Frenando;
                _fuerzaMotor = diferenciaVelocidad / 30;

                // Para evitar retroceso, si este vehiculo tiene velocidad negativa,
                // establecer velocidad a 0
                if (velocidad <= 0.1)
                {
                    _rb.velocity = Vector2.zero;
                    _estado = Estado.Reposo;
                    return;
                }
            }
            // Frenar en seco
            else
            {
                _estado = Estado.Frenando;
                _fuerzaMotor = -4;

                // Para evitar retroceso, si este vehiculo tiene velocidad negativa,
                // establecer velocidad a 0
                if (velocidad <= 0.1)
                {
                    _rb.velocity = Vector2.zero;
                    _estado = Estado.Reposo;
                    return;
                }
            }
        }

        public void SetPuntoDestino(Vector3 destino)
        {
            _puntoDestino = destino;
            Debug.Log("Establecer destino: " + destino);
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