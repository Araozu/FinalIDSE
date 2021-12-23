using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts
{
    internal enum EstadoPersona
    {
        Caminando,
        Detenido,
    }

    public class Persona : MonoBehaviour
    {
        private Vector3 _movimiento = new Vector3(0, 1, 0);
        private Animator _animator;
        private Rigidbody2D _rb;
        private int _framesEnPista = 1;
        private EstadoPersona _estado;

        public GameObject circuloTaxi;

        public bool Cooldown { get; private set; }

        public bool PidiendoTaxi { get; private set; }

        private static readonly int Caminando = Animator.StringToHash("Caminando");

        private void Start()
        {
            Cooldown = true;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            var rotacionInicial = _rb.rotation;
            _movimiento = (rotacionInicial % 360) switch
            {
                90 => new Vector3(-1, 0),
                180 => new Vector3(0, -1),
                270 => new Vector3(1, 0),
                _ => _movimiento
            };

            Mover();
            StartCoroutine(TerminarCooldown());
        }

        private void Update()
        {
            if (_estado == EstadoPersona.Detenido) return;

            ActualizarPosicion();
            if (_framesEnPista > 60)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator TerminarCooldown()
        {
            yield return new WaitForSeconds(5);
            Cooldown = false;
        }

        private void Mover()
        {
            _animator.SetBool(Caminando, true);
            _estado = EstadoPersona.Caminando;
        }

        private void RotarMovimiento()
        {
            var nuevoValor = Random.Range(-1, 1);
            if (_movimiento.x != 0)
            {
                if (nuevoValor < 0)
                {
                    _movimiento = new Vector3(0, -1, 0);
                    _rb.rotation = 180;
                }
                else
                {
                    _movimiento = new Vector3(0, 1, 0);
                    _rb.rotation = 0;
                }
            }
            else
            {
                if (nuevoValor < 0)
                {
                    _movimiento = new Vector3(-1, 0, 0);
                    _rb.rotation = 90;
                }
                else
                {
                    _movimiento = new Vector3(1, 0, 0);
                    _rb.rotation = 270;
                }
            }
        }

        private void DireccionOpuesta()
        {
            _movimiento *= -1;
            transform.position += _movimiento * Time.deltaTime * 15;
            RotarMovimiento();
        }

        private void Detener()
        {
            _animator.SetBool(Caminando, false);
            _estado = EstadoPersona.Detenido;
        }

        private void ActualizarPosicion()
        {
            transform.position += _movimiento * Time.deltaTime;
        }

        public void PedirTaxi()
        {
            Detener();
            PidiendoTaxi = true;
            var circuloTaxiObject = Instantiate(circuloTaxi);
            // No se crea el circulo como hijo de la persona porque esto cambia
            // el tamaño de su rigidbody.
            // En su lugar, guardar una referencia de esta persona en el circulo.
            circuloTaxiObject.GetComponent<CirculoTaxi>().SetPersonaOrigen(this);
            circuloTaxiObject.transform.position = transform.position;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Estacion"))
            {
                col.transform.parent.GetComponent<BusStop>().AumentarPasajero();
                Destroy(gameObject);
                return;
            }

            DireccionOpuesta();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _framesEnPista++;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _framesEnPista = 0;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            DireccionOpuesta();
        }
    }
}