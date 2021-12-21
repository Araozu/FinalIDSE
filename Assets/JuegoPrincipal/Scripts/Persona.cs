using System;
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

        private static readonly int Caminando = Animator.StringToHash("Caminando");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
            var rotacionInicial = _rb.rotation;
            Debug.Log(rotacionInicial);
            _movimiento = (rotacionInicial % 360) switch
            {
                90 => new Vector3(-1, 0),
                180 => new Vector3(0, -1),
                270 => new Vector3(1, 0),
                _ => _movimiento
            };

            Mover();
        }

        private void Update()
        {
            ActualizarPosicion();
        }

        private void Mover()
        {
            _animator.SetBool(Caminando, true);
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
            transform.position += _movimiento * Time.deltaTime * 2;
            RotarMovimiento();
        }

        private void Detener()
        {
            _animator.SetBool(Caminando, false);
        }

        private void ActualizarPosicion()
        {
            transform.position += _movimiento * Time.deltaTime;
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

        private void OnCollisionEnter2D(Collision2D col)
        {
            DireccionOpuesta();
        }
    }
}