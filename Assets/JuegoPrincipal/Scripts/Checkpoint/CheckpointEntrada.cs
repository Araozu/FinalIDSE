using UnityEngine;

namespace JuegoPrincipal.Scripts.Checkpoint
{
    public class CheckpointEntrada : MonoBehaviour
    {
        private PistaController _pistaController;

        private void Start()
        {
            _pistaController = transform.parent.GetComponent<PistaController>();
        }

        public Vector2 ObtenerVectorEntrada()
        {
            var t = transform;
            return t.position - t.parent.position;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Vehiculo")) return;

            var npc = col.GetComponent<NPC>();
            var puntoSalida = _pistaController.GetCheckpointSalida();
            npc.SetPuntoDestino(puntoSalida);
        }
    }
}