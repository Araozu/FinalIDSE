using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/**
 * El velocimetro controla el velocimetro que se muestra en la UI
 */
public class Velocimetro : MonoBehaviour
{
    public JugadorController jugador;
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        // TODO: Actualizar la interfaz con la velocidad del jugador
        var velocidadJugador = jugador.Velocidad();
        _text.text = math.floor(velocidadJugador) + " km/h";
    }
}
