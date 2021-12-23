using System;
using Unity.Mathematics;
using UnityEngine;

/**
 * PistaController colisiona con el vehiculo, y calcula si el sentido
 * del vehículo es correcta.
 * Esto para despues alertar al jugador que esta yendo en sentido contrario.
 */
public class PistaController : MonoBehaviour
{
    private float tiempoSentidoContrario = 0;

    private void OnTriggerStay2D(Collider2D other)
    {
        // Si la colision no es con el jugador retornar
        if (!other.CompareTag("Player")) return;

        var upVehiculo = other.transform.up;
        var upPista = transform.up;

        var diferenciaX = math.abs(upVehiculo.x + upPista.x);
        var diferenciaY = math.abs(upVehiculo.y + upPista.y);

        var maximo = math.max(diferenciaX, diferenciaY);

        if (maximo < 0.8)
        {
            tiempoSentidoContrario += Time.deltaTime;
            if (tiempoSentidoContrario > 2)
            {
                tiempoSentidoContrario -= 1;
                other.GetComponent<JugadorController>().HacerDanoPorSentidoContrario();
            }
        }
    }

    /**
     * Devuelvue la posicion del punto en el que acaba la pista
     */
    public Vector3 GetCheckpointSalida()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("CheckpointSalida"))
            {
                return child.transform.position;
            }
        }

        Debug.LogError("PistaController - GetCheckpointSalida : No se encontro un hijo " +
                       "con etiqueta CheckpointSalida.");
        return Vector3.zero;
    }
}