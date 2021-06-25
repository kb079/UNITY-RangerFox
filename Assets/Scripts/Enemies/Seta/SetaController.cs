using System.Collections;
using UnityEngine;

public class SetaController : Enemy
{
    [SerializeField] int maxDis = 20;
    [SerializeField] GameObject objAtk;
    public Transform negZ, posZ, negX, posX;
    public bool hasLimits;
    private bool isActive = false, isHiding = true, flag_attack = false, flag_prepare_unhide = false, flag_unhide = false, flag_hide = false;
    private float nZ, pZ, nX, pX, rad = 8f, playerRad = 2f;
    public GameObject[] animations;
    public AudioSource audiosource;
    public AudioClip[] audios = new AudioClip[4];

    private void Start()
    {
        health = GameConstants.Seta_HP;
        if (hasLimits)
        {
            nZ = negZ.position.z;
            pZ = posZ.position.z;
            nX = negX.position.x;
            pX = posX.position.x;
        }
        //se coloca por debajo del terreno
        transform.position = new Vector3(transform.position.x, -10, transform.position.z);
    }

    protected override void Update()
    {
        if (!isDead)
        {
            checkIfActive();
            if (isActive)
            {
                if (!audiosource.isPlaying && isHiding == false)
                {
                    audiosource.PlayOneShot(audios[1]);
                }
                
                actions();
            }
        }

        else
        { 
            StopAllCoroutines();
            audiosource.PlayOneShot(audios[3], 0.5f);
        }
    }

    private void changeAnimation(int id)
    {
        animations[0].SetActive(false);
        animations[1].SetActive(false);
        animations[2].SetActive(false);
        animations[3].SetActive(false);
        animations[4].SetActive(false);
        animations[5].SetActive(false);

        animations[id].SetActive(true);
    }

    private void checkIfActive()
    {
        Vector3 pos1 = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 pos2 = new Vector3(player.transform.position.x, 0, player.transform.position.z);

        int distance = (int)Vector3.Distance(pos1, pos2);
        //activar al acercarse a la distancia establecida
        if (distance <= maxDis && !isActive)
        {
            //el enemigo aparece en la �ltima posici�n en la que qued�
            changeAnimation(4);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            StartCoroutine(waitTo(2, 0.8f));
            isActive = true;
            
        }
        //desactivar al alejarse de la distancia establecida
        else if (distance > maxDis && isActive)
        {
            //se vuelve a esconder bajo tierra
            transform.position = new Vector3(transform.position.x, -10, transform.position.z);
            StopAllCoroutines();
            flag_attack = false;
            flag_hide = false;
            flag_prepare_unhide = false;
            flag_unhide = false;
            isActive = false;
        }
    }

    private void actions()
    {
        //si no esta escondido apuntara a la direccion del jugador, asi se evitan errores de rotacion al acercarse demasiado
        //las balas se disparan en dicha direccion
        if (!isHiding) transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        if (flag_prepare_unhide)
        {
            changeAnimation(4);
            flag_prepare_unhide = false;
            prepareUnhiding();
            //el tiempo est� fijo porque la animaci�n de salir de la tierra siempre es igual
            StartCoroutine(waitTo(2, 0.8f));
        }
        else if (flag_unhide)
        {
            changeAnimation(5);
            flag_unhide = false;
            unhide();
            audiosource.PlayOneShot(audios[0], 0.6f);
            float time = Random.Range(2f, 4f);
            StartCoroutine(waitTo(0, time));
            
        }
        else if (flag_attack)
        {
            changeAnimation(3);
            flag_attack = false;
            
            //float time = Random.Range(1.5f, 2.5f);
            StartCoroutine(waitTo(5, 1f));
        }
        else if (flag_hide)
        {
            changeAnimation(2);
            flag_hide = false;
            hide_1();
            audiosource.PlayOneShot(audios[0], 0.6f);
            float time = Random.Range(2.5f, 3.5f);
            StartCoroutine(waitTo(3, time));
        }
    }

    //controla el tiempo entre cada acci�n
    IEnumerator waitTo(int caseId, float time)
    {
        yield return new WaitForSeconds(time);
        //0: attack, 1: hide, 2: unhide, 3: prepare unhiding, 4: hide 1 to 2, 5: prepare attack animation, 6: change to idle
        switch (caseId)
        {
            case 0:
                flag_attack = true;
                break;
            case 1:
                flag_hide = true;
                break;
            case 2:
                flag_unhide = true;
                break;
            case 3:
                flag_prepare_unhide = true;
                break;
            case 4:
                hide_2();
                break;
            case 5:
                attack();
                StartCoroutine(waitTo(1, Random.Range(2.5f, 3.5f)));
                break;
            case 6:
                changeAnimation(0);
                break;
            default:
                break;
        }
    }

    protected override void attack()
    {
        GameObject attackClone = Instantiate(objAtk, objAtk.transform.position, transform.rotation);
        attackClone.SetActive(true);
        StartCoroutine(waitTo(6, 0.8f));
        audiosource.PlayOneShot(audios[2]);
    }

    private void hide_1()
    {
        StartCoroutine(waitTo(4, 0.8f));
    }

    private void hide_2()
    {
        transform.position = new Vector3(transform.position.x, -10, transform.position.z);
        isHiding = true;
    }

    private void unhide()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        isHiding = false;
        StartCoroutine(waitTo(6, 0.7f));
    }

    private void prepareUnhiding()
    {
        //radio disponible por el que saldr�
        Vector2 circle = Random.insideUnitCircle * rad;
        float r1 = circle.x;
        float r2 = circle.y;

        //para evitar que salga justo debajo del jugador al resultado se le suma un radio establecido
        if (r1 < 0) r1 -= playerRad; else r1 += playerRad;
        if (r2 < 0) r2 -= playerRad; else r2 += playerRad;

        float newX = player.transform.position.x + r1;
        float newZ = player.transform.position.z + r2;

        //para evitar que se salga de los l�mites (si estan activados)
        if (hasLimits)
        {
            if (newX > pX) newX = pX;
            else if (newX < nX) newX = nX;
            if (newZ > pZ) newZ = pZ;
            else if (newZ < nZ) newZ = nZ;
        }

        transform.position = new Vector3(newX, 0, newZ);
    }


    protected override void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            changeAnimation(1);
            StartCoroutine(dropItem(3f));
            PlayerStats.getInstance().addXP(GameConstants.Seta_Exp);
        }
    }
}


//--------------------------------------------------------------
//para activar/desactivar los transform en el inspector de unity
//--------------------------------------------------------------
/*[CustomEditor(typeof(SetaController))]
public class EffectsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        SetaController seta = (SetaController)target;
        seta.hasLimits = EditorGUILayout.Toggle("Has Limits", seta.hasLimits);

        if (seta.hasLimits)
        {
            //EditorGUILayout.field;
            EditorGUILayout.ObjectField("-Z Limit", seta.negZ, typeof(Transform), true);
            EditorGUILayout.ObjectField("+Z Limit", seta.posZ, typeof(Transform), true);
            EditorGUILayout.ObjectField("-X Limit", seta.negX, typeof(Transform), true);
            EditorGUILayout.ObjectField("+X Limit", seta.posX, typeof(Transform), true);
        }
    }
}*/

/*enum animationEnum
{
    IDLE = 0,
    DEAD = 1,
    HIDE = 2,
    ATTACK = 3,
    UNHIDE_1 = 4,
    UNHIDE_2 = 5
}*/