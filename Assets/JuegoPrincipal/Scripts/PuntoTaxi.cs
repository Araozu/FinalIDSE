using System.Collections;
using UnityEngine;

namespace JuegoPrincipal.Scripts
{
    public class PuntoTaxi : MonoBehaviour
    {
        public GameObject persona;
        private SpriteRenderer _renderer;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private IEnumerator BajarPasajero(JugadorController jugador, TaxiScript taxi)
        {
            // Desactivar movimiento
            jugador.DesactivarMovimiento();

            yield return new WaitForSeconds(1);

            // Crear una persona en el punto de salida
            var personaGameObject = Instantiate(persona);
            personaGameObject.transform.position = transform.GetChild(0).transform.position;

            yield return new WaitForSeconds(1);

            // Activar movimiento
            jugador.ActivarMovimiento();
            
            Desactivar();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.GetComponent<Rigidbody2D>().velocity != Vector2.zero) return;

            var taxi = other.GetComponent<TaxiScript>();
            if (!taxi.TienePasajero) return;
            taxi.BajarPasajero();

            var jugador = other.GetComponent<JugadorController>();

            StartCoroutine(BajarPasajero(jugador, taxi));
        }

        private void Desactivar()
        {
            _renderer.color = new Color(255, 255, 255, 0);
        }
        
        public void Activar()
        {
            _renderer.color = new Color(255, 255, 255, 1);
        }
    }
}