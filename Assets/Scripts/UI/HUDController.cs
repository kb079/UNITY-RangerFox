using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image hpBar;
    public Image mpBar;
    public Image staminaBar;

    //Para animaci�n barra hp
    private float currentHealth;
    private float currentAuxHealth;
    private float auxHealth;
    private bool animatedHpBar = false;

    //Para animaci�n barra mp
    private float currentMana;
    private float currentAuxMana;
    private float auxMana;
    private bool animatedMpBar = false;

    public Text hudText;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //Para animaci�n barras
        currentHealth = player.maxHealth;
        currentMana = player.maxMana;

        hpBar.fillAmount = 1;
        mpBar.fillAmount = 1;
    }

    void Update()
    {
        //Si nota un cambio en la vida del jugador
        if (player.getHealth() != currentHealth)
        {
            //axuHealth es la diferencia entre la vida que ten�a y la vida que tiene ahora
            auxHealth = player.getHealth() - currentHealth;

            //si hay que quitar vida
            if (player.getHealth() < currentHealth)
            {
                currentHealth = player.getHealth();
                currentAuxHealth = currentHealth - auxHealth;
            }
            //si hay que a�adir vida
            else
            {
                currentAuxHealth = currentHealth;
                currentHealth = player.getHealth();
            }
            //para activar la animaci�n
            animatedHpBar = true;
            StartCoroutine(controlHpAnimation());
            StartCoroutine(hpBarAnimation());
        }

        //Si nota un cambio en el man� del jugador
        if (player.getMana() != currentMana)
        {
            //axuMana es la diferencia entre el man� que ten�a y el man� que tiene ahora
            auxMana = player.getMana() - currentMana;

            //si hay que quitar man�
            if (player.getMana() < currentMana)
            {
                currentMana = player.getMana();
                currentAuxMana = currentMana - auxMana;
            }
            //si hay que a�adir man�
            else
            {
                currentAuxMana = currentMana;
                currentMana = player.getMana();
            }

            //para activar la animaci�n
            animatedMpBar = true;
            StartCoroutine(controlMpAnimation());
            StartCoroutine(mpBarAnimation());
        }

        //la stamina se actualiza a cada momento, as� como el texto en pantalla
        float sp = player.getStamina() / player.maxStamina;
        staminaBar.fillAmount = sp;

        hudText.text = player.getHudText();
    }

    //cuando se acabe, terminar� la animaci�n de la barra y quedar� con el valor exacto que deber�a tener
    IEnumerator controlHpAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        animatedHpBar = false;
        float hp = currentHealth / player.maxHealth;
        hpBar.fillAmount = hp;
    }

    //mientras el bool sea true, se actualizar� la barra con el valor anterior - el da�o quitado / 2
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

    //cuando se acabe, terminar� la animaci�n de la barra y quedar� con el valor exacto que deber�a tener
    IEnumerator controlMpAnimation()
    {
        yield return new WaitForSeconds(0.6f);
        animatedMpBar = false;
        float mp = currentMana / player.maxMana;
        mpBar.fillAmount = mp;
    }

    //mientras el bool sea true, se actualizar� la barra con el valor anterior - el man� quitado / 2
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