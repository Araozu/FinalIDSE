using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace JuegoPrincipal.Scripts.Checkpoint
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckpointSalida : MonoBehaviour
    {
        private Vector2 ObtenerVectorSalida()
        {
            var t = transform;
            return t.parent.position - t.position;
        }

        private static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
        {
            var vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
            var sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
            return Vector2.Angle(vec1, vec2) * sign;
        }

        // Cuando un npc colisiona con su punto destino
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag("Vehiculo")) return;

            var npc = col.GetComponent<NPC>();

            // Buscar otros checkpointEntrada en un radio.
            var objetos = Physics2D.OverlapCircleAll(transform.position, 10f);
            var puntosDestino = new List<Collider2D>();
            foreach (var objeto in objetos)
            {
                if (!objeto.CompareTag("CheckpointEntrada")) continue;

                var checkpointEntrada = objeto.GetComponent<CheckpointEntrada>();
                var anguloDiferencia =
                    AngleBetweenVector2(checkpointEntrada.ObtenerVectorEntrada(), ObtenerVectorSalida());

                // Si el punto entrada da una vuelta, descartarlo
                if (anguloDiferencia > 150) continue;

                puntosDestino.Add(objeto);
            }

            // Escoger un punto al azar y asignarlo como destino del vehiculo
            var indiceDestino = Random.Range(0, puntosDestino.Count);
            var puntoDestino = puntosDestino[indiceDestino];
            npc.SetPuntoDestino(puntoDestino.transform.position);
        }
    }
}
