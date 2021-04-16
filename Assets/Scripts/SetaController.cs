using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetaController : Enemy
{
    private GameObject player;
    private Player playerComponent;
    private bool isActive = false, isDead = false, flag_attack = false, isHiding = true;
    [SerializeField] int health = 15, maxDis = 20;
    [SerializeField] GameObject objAtk;
    [SerializeField] Transform negZ, posZ, negX, posX;
    private float nZ, pZ, nX, pX;

    private void Start()
    {
        nZ = negZ.position.z;
        pZ = posZ.position.z;
        nX = negX.position.x;
        pX = posX.position.x;
        player = GameObject.FindGameObjectWithTag("Player");
        playerComponent = player.GetComponent<Player>();
        transform.position = new Vector3(transform.position.x, -10, transform.position.z);
    }

    void Update()
    {
        if (!isDead)
        {
            activate();
            if (isActive)
            {
                action();
            }
        } else
        {
            StopAllCoroutines();
        }
    }

    private void activate()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = player.transform.position;

        int distance = (int)Vector3.Distance(pos1, pos2);
        //debug*
        playerComponent.setHudText(distance.ToString());
        //*
        //activar
        if (distance <= maxDis && !isActive)
        {
            StartCoroutine(waitTo(3, 0.1f));
            isActive = true;
        } 
        //desactivar
        else if (distance > maxDis && isActive){
            transform.position = new Vector3(transform.position.x, -10, transform.position.z);
            isActive = false;
            StopAllCoroutines();
            flag_attack = false;
        }
    }

    private void action()
    {
        if(!isHiding) transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
        if (flag_attack)
        {
            flag_attack = false;
            float time = Random.Range(2f, 4f);
            StartCoroutine(waitTo(0, time));
        }
    }

    IEnumerator waitTo(int caseId, float time)
    {
        yield return new WaitForSeconds(time);
        //0: attack, 1: hide, 2: unhide, 3: prepare unhiding
        switch (caseId)
        {
            case 0:
                attack();
                break;
            case 1:
                hide();
                break;
            case 2:
                unhide();
                break;
            case 3:
                prepareUnhiding();
                break;
        }
    }

    private void attack()
    {
        GameObject attackClone = Instantiate(objAtk, objAtk.transform.position, transform.rotation);
        attackClone.SetActive(true);
        float time = Random.Range(1.5f, 2.5f);
        StartCoroutine(waitTo(1, time));
    }

    private void hide()
    {
        transform.position = new Vector3(transform.position.x, -10, transform.position.z);
        isHiding = true;
        float time = Random.Range(2f, 4f);
        StartCoroutine(waitTo(3, time));
    }

    private void unhide()
    {
        flag_attack = true;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        isHiding = false;
    }

    private void prepareUnhiding()
    {
        //radio disponible por el que saldrá
        float r1 = Random.Range(-10f, 10f);
        float r2 = Random.Range(-10f, 10f);

        //para evitar que salga justo debajo del jugador
        //if (r1 < 2 && r1 > -2) r1 = 2;
        //if (r2 < 2 && r2 > -2) r2 = -2;

        float newX = player.transform.position.x + r1;
        float newZ = player.transform.position.z + r2;

        //para evitar que se salga de los límites
        if (newX >= pX || newX <= nX) newX = nX;
        if (newZ >= pZ || newZ <= nZ) newZ = nZ;

        transform.position = new Vector3(newX, -3, newZ);

        StartCoroutine(waitTo(2, 0.8f));
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
