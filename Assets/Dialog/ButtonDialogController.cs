using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDialogController : MonoBehaviour
{
    
    public void changeColor()
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
    }
    public void restartColor()
    {
        GetComponent<Image>().color = Color.black;
    }

}
