using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * El velocimetro controla el velocimetro que se muestra en la UI
 */
public class Velocimetro : MonoBehaviour
{
    public JugadorController jugador;

    private void Update()
    {
        // TODO: Actualizar la interfaz con la velocidad del jugador
        var velocidadJugador = jugador.GetVelocity();
    }
}
