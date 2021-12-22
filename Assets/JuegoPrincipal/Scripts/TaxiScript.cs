using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts
{
    public class TaxiScript : MonoBehaviour
    {
        public bool TienePasajero { get; private set; }
        private float _puntoSuerte = 0.2f;

        private PuntoTaxi[] _puntosTaxis;
        private Vector3 _objetivo = Vector3.zero;
        private FlechaScript _flechaScript;

        private void Start()
        {
            StartCoroutine(BuscarPasajero());
            _puntosTaxis = FindObjectsOfType<PuntoTaxi>();
            _flechaScript = GetComponentInChildren<FlechaScript>();
        }

        private IEnumerator BuscarPasajero()
        {
            if (TienePasajero) yield break;

            var deberiaBuscarPasajero = Random.Range(0f, 1f) < _puntoSuerte;

            if (deberiaBuscarPasajero)
            {
                // Buscar personas en un area
                var objetos = Physics2D.OverlapCircleAll(transform.position, 10f);
                foreach (var objeto in objetos)
                {
                    var persona = objeto.GetComponent<Persona>();
                    if (persona == null) continue;
                    if (persona.PidiendoTaxi) continue;

                    persona.PedirTaxi();
                    _puntoSuerte = 0.2f;
                    break;
                }
            }
            else
            {
                // Aumentar la posibilidad de buscar pasajero en 10%
                _puntoSuerte += 0.1f;
            }

            yield return new WaitForSeconds(3);
            StartCoroutine(BuscarPasajero());
        }

        public void SubirPasajero()
        {
            Debug.Log("Pasajero Aceptado");
            TienePasajero = true;
            _puntoSuerte = 0.2f;
            
            var indiceParada = Random.Range(0, _puntosTaxis.Length);
            var parada = _puntosTaxis[indiceParada];
            var paradaPosition = parada.transform.position;
            _objetivo = paradaPosition;
            _flechaScript.SetTarget(paradaPosition);
            parada.Activar();
        }

        public void BajarPasajero()
        {
            TienePasajero = false;
            StartCoroutine(BuscarPasajero());
        }
    }
}