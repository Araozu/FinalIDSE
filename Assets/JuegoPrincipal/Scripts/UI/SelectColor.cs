using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectColor : MonoBehaviour
{
    // Start is called before the first frame update
   /* public Color wantedColor;
    public Button button;*/
    [SerializeField] int count = 0;
    [SerializeField] Color [] colorArray;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeButtonColor(){
      /*  ColorBlock cb = button.colors;
        cb.normalColor = wantedColor;
        cb.highlightedColor = wantedColor;
        cb.pressedColor = wantedColor;
        button.colors = cb;*/
    }
    public void ChangeColor()
    {
        //gameObject.GetComponent<Image>().color = Color.red;

        if(count<colorArray.Length)
        {
            gameObject.GetComponent<Image>().color = new Color(colorArray[count].r,colorArray[count].g, colorArray[count].b, colorArray[count].a);
            if(count == colorArray.Length-1)
            {
                count = -1;
            }
            count+=1;
        }



    }
}
