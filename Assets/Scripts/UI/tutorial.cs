using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{

    //este script se pone en el text dondee quieres que aparezca cada mensaje en este caso TextIndications iria que seria el texto donde aparece cada 1
    private Text text;
    [TextArea]
    public string[] indications;
    public bool isShowingUp;
    public int cont;
    private float time = 1F;

    // Start is called before the first frame update
    void Start()
    {
        //empieza en la posicion 0 y a partir de hay va subiendo el contador que le suma 1 a cada iteracion que haga
        text = gameObject.GetComponent<Text>();
        text.text = indications[0];
        isShowingUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowingUp)
        {
              //si apreta w aparece 1 texto y asi con cada texto y el contador se le suma 1
            if (Input.GetKeyDown(KeyCode.W) && cont == 0)
            {

                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.S) && cont == 1)
            {

                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.D) && cont == 2)
            {

                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.A) && cont == 3)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.Mouse0) && cont == 4)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && cont == 5)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && cont == 6)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.E) && cont == 7)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && cont == 8)
            {
                StartCoroutine(ShowingText());
            }

        }
        
    }

    //showing text sirve para que enseñe cada texto que se ponga
    IEnumerator ShowingText()
    {
        //al principio showingup es falso ya que no saca nada
        isShowingUp = false;
        text.text = "";
        yield return new WaitForSeconds(time);
        //luego cambia a true para que enseñe y el contador suma 1 apra que cada iteración sea distinta.
        isShowingUp = true;
        cont++;
        text.text = indications[cont];
    }
}
