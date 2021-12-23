using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JuegoPrincipal.Scripts.UI
{
    public class InfraccionScript : MonoBehaviour
    {
        private Text _text;
        private Coroutine _coroutine;
    
        // Start is called before the first frame update
        private void Start()
        {
            _text = GetComponent<Text>();
        }

        private IEnumerator LimpiarRazon()
        {
            yield return new WaitForSeconds(1);
            _text.text = "";
            _coroutine = null;
        }
    
        public IEnumerator SetRazonInfraccion(string razon)
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _text.text = "";
            yield return new WaitForSeconds(0.1f);
            _text.text = razon;
            StartCoroutine(LimpiarRazon());
        }
    }
}
