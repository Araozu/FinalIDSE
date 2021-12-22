using UnityEngine;

namespace JuegoPrincipal.Scripts
{
    public class CirculoTaxi : MonoBehaviour
    {
        private Persona personaOrigen;

        public void SetPersonaOrigen(Persona persona)
        {
            personaOrigen = persona;
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (other.GetComponent<Rigidbody2D>().velocity != Vector2.zero) return;

            // Al colisionar con el jugador, destruir la persona y hacer que el
            // taxi este ocupado
            var taxi = other.GetComponent<TaxiScript>();
            taxi.SubirPasajero();
            Destroy(personaOrigen.gameObject);
            Destroy(gameObject);
        }
    }
}
