using System;
using UnityEngine;

namespace JuegoPrincipal.Scripts
{
    public class NPCDetector : MonoBehaviour
    {
        private NPC _npc;

        private void Start()
        {
            _npc = GetComponentInParent<NPC>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            // Solo detectar otros vehiculos y al jugador
            if (other.CompareTag("Vehiculo") || other.CompareTag("Player"))
            {
                _npc.Frenar();
            }
        }
    }
}
