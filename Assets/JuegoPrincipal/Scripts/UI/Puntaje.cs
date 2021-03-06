using System;
using UnityEngine;
using UnityEngine.UI;

namespace JuegoPrincipal.Scripts.UI
{
    public class Puntaje : MonoBehaviour
    {

        private Text _text;
        public int Puntos { get; private set; }
        private InfraccionScript _infraccionScript;
        public Canvas canvasgano;
        public Canvas canvasperdio;
    
        private void Start()
        {
            _text = GetComponent<Text>();
            _infraccionScript = FindObjectOfType<InfraccionScript>();
            Puntos = 20;
        }

        private void Update()
        {
            if (Puntos <= 0)
            {
                // TODO game over
                canvasperdio.gameObject.SetActive(true); 
                Debug.Log("game over");
                return;
            }
            if (Puntos >= 100)
            {
                // TODO pantalla ganador
                canvasgano.gameObject.SetActive(true); 
                Debug.Log("ganaste");
                return;
            }

            if (Puntos > 20)
            {
                _text.color = Color.green;
            }
            else if (Puntos > 10 && Puntos < 20)
            {
                _text.color = new Color(255, 100, 0);
            }
            else if (Puntos < 10)
            {
                _text.color = Color.red;
            }
            
            _text.text = Puntos + " pts";
        }

        public void SumarPuntos(int cantidad)
        {
            Puntos += cantidad;
        }
 
        public void RestarPuntos(int cantidad, string razon)
        {
            Puntos -= cantidad;
            StartCoroutine(_infraccionScript.SetRazonInfraccion("-" + cantidad + " pts por " + razon + "!"));
        }
    }
}
