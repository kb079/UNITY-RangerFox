using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Incoporamos Text Mesh Pro
using TMPro;

public class DialogManager : MonoBehaviour
{
    /*
     * El _dialogBox es opcional, es una caja
     * que rodea al texto, el script solo
     * la activa y desactiva. 
     */
    [System.Serializable]
    public struct _dialogBox
    {
        public int _character; //Personaje al que pertenece. 
        public GameObject _box; //Objeto a activar / desacticar 
    }
    private float _timeNextWrite, _timeLastWrite;
    /*
     * _sentence. El dialogo se construye en base a un array
     * de esta estructura
     */
    [System.Serializable]
    public struct _sentence
    {
        public int _character; //Caracter al que pertence la frase. 
        public string _2say; //Lo que tiene que decir. 
        public bool _active; //Indica si se tiene que activar el marco de dialogo. 
        public bool _deactive; //Se desactivara el marco de dialogo en la siguiente frase.
        public float _timeActive; //Tiempo maximo que esta cada frase activa
        public GameObject _o2ActiveEnd;
    }

    public TextMeshProUGUI[] _textContainers; //Array de TextMeshPro en los que se muestra el texto 
    public _sentence[] _sentences; //Array de frases que diran nuestros protagonistas
    public _dialogBox[] _dialogBoxes; //El marco que puede mostrarse rodeando las frases. 

    [SerializeField]
    private float _speedWrite; //Tiempo que pasa entre que se escribe una letra y la otra. 

    //private bool _enableDisableBox = false; 
    private int _index = 0; //Contador interno para saber por que frase vamos. 


    private void Start()
    {
        CleanText(); //Limpiamos el texto que se pueda encontrar en los textmeshpro
        if (_sentences[_index]._active == true)
        {
            //En el caso de que la primera frase lo indique activamos el dialogbox. 
            EnableDisableBox(true);
        }
        //Empezamos la conversacion. 
        StartCoroutine(Talk());
        if (_sentences[_index]._timeActive > 0)
        {
            Invoke("ContinueDialog", _sentences[_index]._timeActive);
        }
    }

    private void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {*/
            /*En el caso de que se detecte una pulsacion
            de mouse se continua el dialogo. */

            /*ContinueDialog();
        }*/
    }

    public void CleanText()
    {
        //Eliminamos el texto de todos los TextMeshPro
        for (int i = 0; i < _textContainers.Length; i++)
        {
            _textContainers[i].text = "";
        }
    }

    /*
     * Esta funcion se ejecuta en paralelo.
     * Escribe las frases letra por letra. 
     * */
    IEnumerator Talk()
    {
        
        //Elimina el texto previo. 
        _textContainers[_sentences[_index]._character].text = "";
        foreach (char letra in _sentences[_index]._2say.ToCharArray())
        {
            //Suma cada letra al TextMeshPro
            _textContainers[_sentences[_index]._character].text += letra;
            //La funcion se espera el tiempo indicado.
            yield return new WaitForSeconds(_speedWrite);
        }

        _speedWrite = 0.015f;
    }
    /*
     * Activa o desactiva el elemento que puede contener el texto
     * y que se lo indicamos en el editor
     * */
    public void EnableDisableBox(bool enable)
    {
        _dialogBoxes[_sentences[_index]._character]._box.SetActive(enable);
    }
    /*
     * Continua con la siguente frase del dialogo
     */
    public float ContinueDialog()
    {
        _speedWrite = 0.05f;
        Debug.Log(_index);
        
        float tiempoEspera = 0;
        if (_index < _sentences.Length)
        {
            if (_sentences[_index]._deactive == true)
            {
                //Si la frase anterios tenia que ser desactivada llamamos a
                //la funcion que desactiva su globo 
                EnableDisableBox(false);
            }
            //Aumentamos en 1 el _index con lo que pasamos a la siguiente frase. 
            _index += 1;
        }

        //Comprobamos que tenemos otra sentencia que escribir
        if (_index < _sentences.Length)
        {
            Debug.Log(_index);
            if (_sentences[_index]._active == true)
            {
                //Si es necesario activamos su dialog box
                
                EnableDisableBox(true);
            }
            tiempoEspera = _sentences[_index]._2say.Length * (_speedWrite * 10);
            Debug.Log(tiempoEspera);
            if(gameObject.active == true)
            StartCoroutine(Talk());
           
            if (_sentences[_index]._o2ActiveEnd != null)
            {
                //Si es necesario activamos su dialog box
                
                _sentences[_index]._o2ActiveEnd.SetActive(true);
            }
            if (_sentences[_index]._timeActive > 0)
            {
                
                Invoke("ContinueDialog", _sentences[_index]._timeActive);
            }
        }
        else
        {
            Debug.Log(_index);
            //si es la ultima frase, limpiamos el texto.
            Debug.Log("No quedan frases");
            CleanText();
            //_dialogBoxes[_sentences[_index]._character]._box.SetActive(false);
            gameObject.SetActive(false);
            
        }
        return tiempoEspera;
    }

    public void passDialog()
    {
        if(_speedWrite == 0.015f)
        {
            StopCoroutine(Talk());
            _speedWrite = 0.05f;
            ContinueDialog();
        }

        else if (_speedWrite == 0.05f)
            _speedWrite = 0.015f;

    }
        
       
    
}

