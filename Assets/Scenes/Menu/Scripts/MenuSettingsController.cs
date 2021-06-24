using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuSettingsController : MonoBehaviour
{
    public Button correr, atacar, magia, barrera, apuntar, interact, dash, inv1, inv2, inv3, inv4, inv5;
    public Slider volume, sens;

    private bool recordingKey;
    private Button clickedBttn;
    private string previousText;
    private KeyCode key;

    private void Start()
    {
        loadSettings();
    }

    private void loadSettings()
    {
        clickedBttn = null;
        previousText = null;
        recordingKey = false;
        key = KeyCode.None;
        
        correr.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_run.ToString());
        correr.onClick.AddListener(delegate { recordKey(correr); });

        atacar.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_attack.ToString());
        atacar.onClick.AddListener(delegate { recordKey(atacar); });

        magia.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_magic.ToString());
        magia.onClick.AddListener(delegate { recordKey(magia); });

        barrera.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_barrier.ToString());
        barrera.onClick.AddListener(delegate { recordKey(barrera); });

        apuntar.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_cameraZoom.ToString());
        apuntar.onClick.AddListener(delegate { recordKey(apuntar); });

        interact.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_interact.ToString());
        interact.onClick.AddListener(delegate { recordKey(interact); });

        dash.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_dash.ToString());
        dash.onClick.AddListener(delegate { recordKey(dash); });

        inv1.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_inv1.ToString());
        inv1.onClick.AddListener(delegate { recordKey(inv1); });

        inv2.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_inv2.ToString());
        inv2.onClick.AddListener(delegate { recordKey(inv2); });

        inv3.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_inv3.ToString());
        inv3.onClick.AddListener(delegate { recordKey(inv3); });

        inv4.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_inv4.ToString());
        inv4.onClick.AddListener(delegate { recordKey(inv4); });

        inv5.GetComponentInChildren<Text>().text = translateKeyText(GameConstants.key_inv5.ToString());
        inv5.onClick.AddListener(delegate { recordKey(inv5); });

        volume.value = GameConstants.volume/100;
        volume.GetComponentInChildren<Text>().text = volume.value + "%";

        sens.value = GameConstants.camMovementSpeed;
        sens.GetComponentInChildren<Text>().text = sens.value.ToString("0.00");
    }

    private string translateKeyText(string key)
    {
        if (key.Equals("Mouse0")){
            key = "CLICK IZQUIERDO";
        }else if (key.Equals("Mouse1")){
            key = "CLICK DERECHO";
        }else if (key.Equals("Mouse2")){
            key = "CLICK MEDIO";
        }else if (key.Contains("Alpha"))
        {
            key = key.Replace("Alpha", "");
        }

        return key;
    }
    private void recordKey(Button clickedBttn)
    {
        if(this.clickedBttn != null && this.clickedBttn != clickedBttn)
        {
            endAction(true, this.clickedBttn);
        }
        this.clickedBttn = clickedBttn;
        previousText = clickedBttn.GetComponentInChildren<Text>().text;
        clickedBttn.interactable = false;

        StartCoroutine(blindingText());

        key = KeyCode.None;
        recordingKey = true;
    }

    private IEnumerator blindingText()
    {
        while (clickedBttn != null)
        {
            clickedBttn.GetComponentInChildren<Text>().text = "PULSA UNA TECLA";
            yield return new WaitForSeconds(0.7f);
            if(clickedBttn != null) clickedBttn.GetComponentInChildren<Text>().text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator finishClick(float time, Button clickedBttn)
    {
        yield return new WaitForSeconds(time);
        clickedBttn.onClick.AddListener(delegate { recordKey(clickedBttn); });
    }

    private void endAction(bool cancelled, Button clickedBttn)
    {
        recordingKey = false;

        clickedBttn.interactable = true;

        if (!cancelled)
        {
            clickedBttn.GetComponentInChildren<Text>().text = translateKeyText(key.ToString());
            switch (clickedBttn.name)
            {
                case "correr":
                    GameConstants.key_run = key;
                    break;
                case "atacar":
                    GameConstants.key_attack = key;
                    break;
                case "magia":
                    GameConstants.key_magic = key;
                    break;
                case "barrera":
                    GameConstants.key_barrier = key;
                    break;
                case "apuntar":
                    GameConstants.key_cameraZoom = key;
                    break;
                case "interactuar":
                    GameConstants.key_interact = key;
                    break;
                case "dash":
                    GameConstants.key_dash = key;
                    break;
                case "inv1":
                    GameConstants.key_inv1 = key;
                    break;
                case "inv2":
                    GameConstants.key_inv2 = key;
                    break;
                case "inv3":
                    GameConstants.key_inv3 = key;
                    break;
                case "inv4":
                    GameConstants.key_inv4 = key;
                    break;
                case "inv5":
                    GameConstants.key_inv5 = key;
                    break;
            }
        }
        else
        {
            clickedBttn.GetComponentInChildren<Text>().text = previousText;
        }

        this.clickedBttn = null;
    }

    void OnGUI()
    {
        if (recordingKey)
        {
            if (Event.current.isKey && Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseDown)
            {
                key = Event.current.keyCode;
                if (key == KeyCode.Escape)
                {
                    endAction(true, clickedBttn);
                    return;
                }

                if(key == KeyCode.None && Event.current.type == EventType.MouseDown)
                {
                    if (Input.GetMouseButtonDown(0)) key = KeyCode.Mouse0;   
                    if (Input.GetMouseButtonDown(1)) key = KeyCode.Mouse1;
                    if (Input.GetMouseButtonDown(2)) key = KeyCode.Mouse2;

                    if (key != KeyCode.None)
                    {
                        clickedBttn.onClick.RemoveAllListeners();  
                        StartCoroutine(finishClick(0.5f, clickedBttn));
                        endAction(false, clickedBttn);
                    }
                }else if(key != KeyCode.None) endAction(false, clickedBttn);
            }
        }
    }

    public void updateVolumeValue()
    {
        GameConstants.volume = (int)(volume.value);
        volume.GetComponentInChildren<Text>().text = Mathf.Floor(volume.value) + "%";
    }

    public void updateSensibilyValue()
    {
        GameConstants.camMovementSpeed = sens.value;
        sens.GetComponentInChildren<Text>().text = sens.value.ToString("0.00");
    }
}