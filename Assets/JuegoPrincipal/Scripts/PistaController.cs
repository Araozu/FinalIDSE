using Unity.Mathematics;
using UnityEngine;

/**
 * PistaController colisiona con el vehiculo, y calcula si el sentido
 * del vehículo es correcta.
 * Esto para despues alertar al jugador que esta yendo en sentido contrario.
 */
public class PistaController : MonoBehaviour
{
    
    
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
            Debug.Log("Sentido contrario");
        }
    }
}