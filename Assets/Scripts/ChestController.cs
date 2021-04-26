using System.Collections;
using UnityEngine;

public class ChestController : Enemy
{
    [SerializeField] int health = 15, maxDis = 20;
    [SerializeField] GameObject objAtk;
    public Transform negZ, posZ, negX, posX;
    public bool hasLimits, isOpened = false;
    public GameObject chestDoor;
    private GameObject player;
    private bool isActive = false, isDead = false, isHiding = true, flag_attack = false, flag_unhide = false, flag_hide = false;
    private float nZ, pZ, nX, pX;

    private void Start()
    {
        if (hasLimits)
        {
            nZ = negZ.position.z;
            pZ = posZ.position.z;
            nX = negX.position.x;
            pX = posX.position.x;
        }
        isOpened = false;
        player = GameObject.FindGameObjectWithTag("Player");
        //se coloca por debajo del terreno
        transform.position = new Vector3(transform.position.x, -2, transform.position.z);
    }

    void Update()
    {
        if (!isDead)
        {
            checkIfActive();
            if (isActive)
            {
                actions();
              
            }
            
        }
        else StopAllCoroutines();
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && !isOpened && !isActive)
        {
            c.GetComponent<Player>().setHudText("Press [" + GameConstants.key_interact.ToString() + "] to open chest");
            
        }
    }

    new private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(GameConstants.key_interact))
            {
                isOpened = true;
                c.GetComponent<Player>().doDamage(10);
            }
         
        }
    }

    private void OnTriggerExit(Collider c)
    {
        c.GetComponent<Player>().setHudText("");
    }

    private void checkIfActive()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;
        Vector3 originalPos = chestDoor.transform.eulerAngles;

        int distance = (int)Vector3.Distance(pos1, pos2);
        //activar cuando intenta abrirlo
        if (isOpened && !isActive)
        {
            //el enemigo aparece en la �ltima posici�n en la que qued�
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            StartCoroutine(waitTo(2, 0.8f));
            originalPos.x = -50;
            chestDoor.transform.eulerAngles = originalPos;
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().setHudText("");
            isActive = true;
        }
        //desactivar al alejarse de la distancia establecida
        else if (distance > maxDis && isActive)
        {
            //se vuelve a esconder bajo tierra
            transform.position = new Vector3(transform.position.x, -2, transform.position.z);
            StartCoroutine(waitTo(1, 0.8f));
            originalPos.x = 0;
            chestDoor.transform.eulerAngles = originalPos;
            StopAllCoroutines();
            flag_attack = false;
            flag_hide = false;
            flag_unhide = false;
            isActive = false;
        }
    }

    private void actions()
    {
        //si no esta escondido apuntara a la direccion del jugador, asi se evitan errores de rotacion al acercarse demasiado
        //las balas se disparan en dicha direccion
        if (!isHiding) transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        if (flag_unhide)
        {
            flag_unhide = false;
            unhide();
            float time = Random.Range(2f, 4f);
            StartCoroutine(waitTo(0, time));
        }
        else if (flag_attack)
        {
            flag_attack = false;
            attack();
            float time = Random.Range(1.5f, 2.5f);
            StartCoroutine(waitTo(0, time));
        }
        else if (flag_hide)
        {
            flag_hide = false;
            hide();
            float time = Random.Range(2.5f, 3.5f);
            StartCoroutine(waitTo(2, time));
        }
    }

    //controla el tiempo entre cada acci�n
    IEnumerator waitTo(int caseId, float time)
    {
        yield return new WaitForSeconds(time);
        //0: attack, 1: hide, 2: unhide
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
        }
    }

    private void attack()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;
        int distance = (int)Vector3.Distance(pos1, pos2);
        //desactivar al alejarse de la distancia establecida
        if (distance > 3)
        {
            GameObject attackClone = Instantiate(objAtk, objAtk.transform.position, objAtk.transform.rotation);
            attackClone.SetActive(true);
        }
       
    }

    private void hide()
    {
        transform.position = new Vector3(transform.position.x, -2, transform.position.z);
        isHiding = true;
    }

    private void unhide()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        isHiding = false;
    }


    public override void doDamage(int dmg)
    {
        health -= dmg;
        checkHP();
    }

    private void checkHP()
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            GetComponent<Rigidbody>().isKinematic = true;
            Vector3 originalRot = transform.localEulerAngles;
            originalRot.x = 90;
            transform.localEulerAngles = originalRot;
            Destroy(gameObject, 3);
        }
    }
}


