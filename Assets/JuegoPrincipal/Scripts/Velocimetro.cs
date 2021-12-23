using JuegoPrincipal.Scripts.UI;
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
    private Color colorInicial;
    private float tiempo = 0f;
    private Puntaje _puntaje;

    private void Start()
    {
        _text = GetComponent<Text>();
        colorInicial = _text.color;
        _puntaje = FindObjectOfType<Puntaje>();
    }

    private void Update()
    {
        var velocidadJugador = jugador.Velocidad();
        _text.text = math.floor(velocidadJugador) + " km/h";
        if (velocidadJugador > 30)
        {
            _text.color = Color.red;

            if (velocidadJugador > 40)
            {
                tiempo += Time.deltaTime;
                if (!(tiempo > 2)) return;
                
                tiempo -= 2;
                var cantidadResta = (int) (velocidadJugador - 30) / 10;
                _puntaje.RestarPuntos(cantidadResta, "exceso de velocidad");
            }
        }
        else
        {
            _text.color = colorInicial;
        }
    }
}