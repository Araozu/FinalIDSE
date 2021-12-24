
using UnityEngine;
using UnityEngine.SceneManagement;

public class comenzar : MonoBehaviour
{
     void Start()
    {
        //Debug.Log("LoadSceneA");
    }

    public void LoadA(string scenename)
    {
     
            SceneManager.LoadScene(scenename);
     
    }
    void Update()
    {
        if (Input.GetKey("space"))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
