using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JuegoPrincipal.Scripts.UI
{
    public class TiempoController : MonoBehaviour
    {
        private Text _text;
        private int tiempo = 60;
        public Canvas canvasperdio;

        private void Start()
        {
            _text = GetComponent<Text>();
            StartCoroutine(Temporizador());
        }

        private IEnumerator Temporizador()
        {
            if (tiempo <= 0)
            {
                // TODO: Game over
                canvasperdio.gameObject.SetActive(true); 

                yield break;
            }

            _text.text = tiempo + "s";
            if (tiempo > 20)
            {
                _text.color = Color.green;
            }
            else if (tiempo > 10 && tiempo < 20)
            {
                _text.color = new Color(255, 100, 0);
            }
            else if (tiempo < 10)
            {
                _text.color = Color.red;
            }

            tiempo--;

            yield return new WaitForSeconds(1);

            StartCoroutine(Temporizador());
        }

        public void AumentarTiempo(int cantidad)
        {
            tiempo += cantidad;
        }
    }
}
