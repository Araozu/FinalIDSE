using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts
{
    public class BusStop : MonoBehaviour
    {
        private int _pasajerosEnParada;
        public bool utilizado = false;

        private void Start()
        {
            _pasajerosEnParada = Random.Range(1, 5);
        }

        public void AumentarPasajero()
        {
            _pasajerosEnParada++;
        }

        private IEnumerator Operar(JugadorController jugador, BusScript bus)
        {
            Debug.Log("Inicio");
            // Desactivar movimiento
            jugador.DesactivarMovimiento();

            // Hacer bajar pasajeros
            var pasajerosABajar = bus.PasajerosABajar;
            for (var i = 0; i < pasajerosABajar; i++)
            {
                bus.BajarPasajero();
                // TODO: Animacion de subir pasajero
                yield return new WaitForSeconds(1);
            }

            // Hacer subir pasajeros
            for (var i = 0; i < _pasajerosEnParada; i++)
            {
                bus.SubirPasajero();
                yield return new WaitForSeconds(1);
                // TODO: Animacion de bajar pasajero
            }

            // Modificar pasajeros por bajar
            bus.ActualizarPasajerosABajar();

            // Activar movimiento
            jugador.ActivarMovimiento();

            // Obtener la siguiente parada y asignarla
            bus.SigParada();

            Debug.Log("Fin");
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (utilizado) return;
            if (!other.CompareTag("Player")) return;

            if (other.GetComponent<Rigidbody2D>().velocity != Vector2.zero) return;

            var jugador = other.GetComponent<JugadorController>();
            var bus = other.GetComponent<BusScript>();

            utilizado = true;
            StartCoroutine(Operar(jugador, bus));
        }
    }
}