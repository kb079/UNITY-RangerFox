using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private static HUDController instance;

    public Image hpBar;
    public Image mpBar;
    public Image staminaBar;
    public Text hudText;
    public GameObject statsPanel;

    //Para animación barra hp
    private float currentHealth;
    private float currentAuxHealth;
    private float auxHealth;
    private bool animatedHpBar = false;

    //Para animación barra mp
    private float currentMana;
    private float currentAuxMana;
    private float auxMana;
    private bool animatedMpBar = false;

    private Player player;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public static HUDController getInstance()
    {
        return instance;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //Para animación barras
        currentHealth = player.maxHealth;
        currentMana = player.maxMana;

        hpBar.fillAmount = 1;
        mpBar.fillAmount = 1;
    }

    void Update()
    {
        //Esconder/mostrar ventana de estadísticas
        if (Input.GetKeyDown(KeyCode.X))
        {
            statsPanel.SetActive(!statsPanel.activeInHierarchy);
        }

        //Si nota un cambio en la vida del jugador
        if (player.getHealth() != currentHealth)
        {
            //axuHealth es la diferencia entre la vida que tenía y la vida que tiene ahora
            auxHealth = player.getHealth() - currentHealth;

            //si hay que quitar vida
            if (player.getHealth() < currentHealth)
            {
                currentHealth = player.getHealth();
                currentAuxHealth = currentHealth - auxHealth;
            }
            //si hay que añadir vida
            else
            {
                currentAuxHealth = currentHealth;
                currentHealth = player.getHealth();
            }
            //para activar la animación
            animatedHpBar = true;
            StartCoroutine(controlHpAnimation());
            StartCoroutine(hpBarAnimation());
        }

        //Si nota un cambio en el maná del jugador
        if (player.getMana() != currentMana)
        {
            //axuMana es la diferencia entre el maná que tenía y el maná que tiene ahora
            auxMana = player.getMana() - currentMana;

            //si hay que quitar maná
            if (player.getMana() < currentMana)
            {
                currentMana = player.getMana();
                currentAuxMana = currentMana - auxMana;
            }
            //si hay que añadir maná
            else
            {
                currentAuxMana = currentMana;
                currentMana = player.getMana();
            }

            //para activar la animación
            animatedMpBar = true;
            StartCoroutine(controlMpAnimation());
            StartCoroutine(mpBarAnimation());
        }

        //la stamina se actualiza a cada momento, así como el texto en pantalla
        float sp = player.getStamina() / player.maxStamina;
        staminaBar.fillAmount = sp;

        hudText.text = player.getHudText();
    }

    //cuando se acabe, terminará la animación de la barra y quedará con el valor exacto que debería tener
    IEnumerator controlHpAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        animatedHpBar = false;
        float hp = currentHealth / player.maxHealth;
        hpBar.fillAmount = hp;
    }

    //mientras el bool sea true, se actualizará la barra con el valor anterior - el daño quitado / 2
    //para dar un efecto exponencial
    IEnumerator hpBarAnimation()
    {
        while (animatedHpBar)
        {
            auxHealth /= 2;
            currentAuxHealth += auxHealth;
            float hp = currentAuxHealth / player.maxHealth;
            hpBar.fillAmount = hp;
            yield return new WaitForSeconds(0.03f);
        }
    }

    //cuando se acabe, terminará la animación de la barra y quedará con el valor exacto que debería tener
    IEnumerator controlMpAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        animatedMpBar = false;
        float mp = currentMana / player.maxMana;
        mpBar.fillAmount = mp;
    }

    //mientras el bool sea true, se actualizará la barra con el valor anterior - el maná quitado / 2
    //para dar un efecto exponencial
    IEnumerator mpBarAnimation()
    {
        while (animatedMpBar)
        {
            auxMana /= 2;
            currentAuxMana += auxMana;
            float mp = currentAuxMana / player.maxMana;
            mpBar.fillAmount = mp;
            yield return new WaitForSeconds(0.03f);
        }
    }
}