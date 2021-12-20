using UnityEngine;

namespace JuegoPrincipal.Scripts
{
    public class NPCDetector : MonoBehaviour
    {
        private NPC _npc;
        private Collider2D _parentCollider;

        private void Start()
        {
            _npc = GetComponentInParent<NPC>();
            _parentCollider = transform.parent.GetComponent<BoxCollider2D>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            // Solo detectar otros vehiculos y al jugador
            if (!other.CompareTag("Vehiculo") && !other.CompareTag("Player")) return;

            float otherVelocity;
            if (other.CompareTag("Vehiculo"))
            {
                otherVelocity = other.GetComponent<NPC>().Velocidad();
            }
            else
            {
                otherVelocity = other.GetComponent<JugadorController>().Velocidad();
            }

            var distancia = other.Distance(_parentCollider).distance;
            _npc.SetDistanciaColision(distancia, otherVelocity);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Solo detectar otros vehiculos y al jugador
            if (other.CompareTag("Vehiculo") || other.CompareTag("Player"))
            {
                _npc.SetDistanciaColision(6, 7);
            }
        }

        public void Destruir()
        {
            Destroy(gameObject);
        }
    }
}