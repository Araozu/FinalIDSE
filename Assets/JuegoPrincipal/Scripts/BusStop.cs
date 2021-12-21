using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts
{
    public class BusStop : MonoBehaviour
    {
        private int _pasajerosEnParada;

        private void Start()
        {
            _pasajerosEnParada = Random.Range(1, 10);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (other.GetComponent<Rigidbody2D>().velocity != Vector2.zero) return;

            var jugador = other.GetComponent<JugadorController>();
            var bus = other.GetComponent<BusScript>();
            // Desactivar movimiento
            jugador.DesactivarMovimiento();

            // Hacer bajar pasajeros
            var pasajerosABajar = bus.PasajerosABajar;
            for (var i = 0; i < pasajerosABajar; i++)
            {
                bus.BajarPasajero();
                // TODO: Animacion de subir pasajero
            }

            // Hacer subir pasajeros
            for (var i = 0; i < _pasajerosEnParada; i++)
            {
                bus.SubirPasajero();
                // TODO: Animacion de bajar pasajero
            }

            // Modificar pasajeros por bajar
            bus.ActualizarPasajerosABajar();

            // Activar movimiento
            jugador.ActivarMovimiento();
        }
    }
}