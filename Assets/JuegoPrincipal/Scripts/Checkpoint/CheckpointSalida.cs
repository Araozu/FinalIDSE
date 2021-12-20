using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts.Checkpoint
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckpointSalida : MonoBehaviour
    {
        // Cuando un npc colisiona con su punto destino
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Vehiculo")) return;

            var npc = col.GetComponent<NPC>();

            // Buscar otros checkpointEntrada en un radio.
            var objetos = Physics2D.OverlapCircleAll(transform.position, 20f);
            var puntosDestino = objetos
                .Where(objeto => objeto.CompareTag("CheckpointEntrada"))
                .ToArray();

            // Escoger un punto al azar y asignarlo como destino del vehiculo
            var indiceDestino = Random.Range(0, puntosDestino.Length);
            var puntoDestino = puntosDestino[indiceDestino];
            npc.SetPuntoDestino(puntoDestino.transform.position);
        }
    }
}