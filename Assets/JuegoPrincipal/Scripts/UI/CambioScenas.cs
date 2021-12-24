using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioScenas : MonoBehaviour
{
    public Canvas doorCanvas;

    public void LoadScene(string Scenename)
    {
        SceneManager.LoadScene(Scenename);
    }
    public void ocultar()
    {
        doorCanvas.gameObject.SetActive(false); 
    }
}
