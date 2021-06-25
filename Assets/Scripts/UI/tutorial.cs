using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tutorial : MonoBehaviour
{

    //este script se pone en el text dondee quieres que aparezca cada mensaje en este caso TextIndications iria que seria el texto donde aparece cada 1
    private Text text;
    [TextArea]
    public string[] indications;
    public bool isShowingUp;
    public int cont;
    private float time = 1F;
    public bool funciona = false;
   
    

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
        Debug.Log("hoalslasd  "  +  cont );

        if (isShowingUp)
        {

            if (Input.GetKey(KeyCode.F))
            {
                cambiarEscena();
            }
            //si apreta w aparece 1 texto y asi con cada texto y el contador se le suma 1
            if ((Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)) && cont == 0 || 
               (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && cont == 0 ||
               (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && cont == 0 || 
               (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && cont == 0)
                
            {  
                StartCoroutine(ShowingText());
            }
            if (Input.GetKey(GameConstants.key_run) && cont == 1)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKey(GameConstants.key_dash) && cont == 2)
            {
                StartCoroutine(ShowingText());
            }
            if (cont == 4 && funciona == false)
            {
                StartCoroutine(CambioAutomatico());
                funciona = true;
            }
            if (Input.GetKey(GameConstants.key_cameraZoom) && cont == 6)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKey(GameConstants.key_barrier) && cont == 8)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKey(GameConstants.key_interact) && cont == 10)
            {
                StartCoroutine(ShowingText());
            }
            if (Input.GetKey(GameConstants.key_inv1) && cont == 11)
            {
                StartCoroutine(ShowingText());
            }

        }
        
    }

    public void changeText()
    {
        StartCoroutine(ShowingText());
    }

    public void cambiarEscena()
    {
        SceneManager.LoadSceneAsync("Madriguera");
    }

    //showing text sirve para que enseñe cada texto que se ponga
    public IEnumerator  ShowingText()
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

    IEnumerator CambioAutomatico()
    {
        yield return new WaitForSeconds(1.2F);
        StartCoroutine(ShowingText());    
    }
}
