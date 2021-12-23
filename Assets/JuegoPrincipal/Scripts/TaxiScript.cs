using System.Collections;
using JuegoPrincipal.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts
{
    public class TaxiScript : MonoBehaviour
    {
        public bool TienePasajero { get; private set; }
        private float _puntoSuerte = 0.5f;
        private int ultimaDistancia;

        private PuntoTaxi[] _puntosTaxis;
        public Vector3 _objetivo { get; private set; }
        private FlechaScript _flechaScript;

        private Puntaje _puntaje;
        private TiempoController _tiempoController;

        private void Start()
        {
            StartCoroutine(BuscarPasajero());
            _puntosTaxis = FindObjectsOfType<PuntoTaxi>();
            _flechaScript = GetComponentInChildren<FlechaScript>();
            _puntaje = FindObjectOfType<Puntaje>();
            _tiempoController = FindObjectOfType<TiempoController>();
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
                    if (persona.Cooldown) continue;

                    persona.PedirTaxi();
                    _puntoSuerte = 0.5f;
                    break;
                }
            }
            else
            {
                // Aumentar la posibilidad de buscar pasajero en 10%
                _puntoSuerte += 0.1f;
            }

            yield return new WaitForSeconds(2);
            StartCoroutine(BuscarPasajero());
        }

        public void SubirPasajero()
        {
            TienePasajero = true;
            _puntoSuerte = 0.5f;

            var indiceParada = Random.Range(0, _puntosTaxis.Length);
            var parada = _puntosTaxis[indiceParada];
            var paradaPosition = parada.transform.position;
            _objetivo = paradaPosition;
            ultimaDistancia =(int) Vector3.Distance(transform.position, _objetivo);
            _flechaScript.SetTarget(paradaPosition);
            parada.Activar();
        }

        public void BajarPasajero()
        {
            TienePasajero = false;
            _puntaje.SumarPuntos(ultimaDistancia / 10);
            _tiempoController.AumentarTiempo(ultimaDistancia / 5);
            StartCoroutine(BuscarPasajero());
        }
    }
}