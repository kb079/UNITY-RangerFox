using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    /// <summary>
    /// CÓMO CONFIGURAR EL SCRIPT
    /// 
    /// - enemyTypes: tipos de enemigos que pueden salir. Tienen las mismas probabilidades. Solo 1 por cada tipo.
    /// - numberOfEnemies: número máximo de enemigos que pueden salir en este spawner.
    /// 
    /// 
    /// CÓMO FUNCIONA
    /// 
    /// Al empezar, se crean x cantidad de enemigos en el área especificada. Dicha área es el tamaño visual del
    /// gameObject (la escala x y z, básicamente). Han de darse varias condiciones para que puedan volver a
    /// respawnear:
    ///     - Al menos uno de los enemigos spawneados aquí debe haber muerto
    ///     - El jugador debe estar dentro del área del respawner (representado con su collider)
    ///     - Han de haber pasado una cantidad x de segundos desde que respawnearon por última vez (corrutina waitToEnableRespawn)
    /// </summary>

    public Material transparente;
    public List<GameObject> enemyTypes = new List<GameObject>();
    public int numberOfEnemies = 3;
    public bool flag_create_enemy = false;

    private bool canRespawn = false;
    private bool isInsideArea = false;
    private EnemySpawn thisSpawner;
    private Vector3 downLeftCorner;
    private float xPlacement;
    private float zPlacement;
    void Start()
    {
        //Le doy una referencia a este spawner a los enemigos creados
        thisSpawner = GetComponent<EnemySpawn>();

        //Escondo el objeto al iniciar
        GetComponent<MeshRenderer>().material = transparente;

        //Asigno las esquinas del objeto
        xPlacement = transform.localScale.x / 2;
        zPlacement = transform.localScale.z / 2;
        downLeftCorner = new Vector3(transform.position.x - xPlacement, transform.position.y, transform.position.z - zPlacement);

        //Los pongo con su valor original, pues se crea dentro de un cuadrado creado desde la esquina inferior izquierda
        xPlacement *= 2;
        zPlacement *= 2;

        //Creo los primeros enemigos
        createEnemies();

        //Tras 10 segundos ya podrá sascar respawns
        StartCoroutine(waitToEnableRespawn());
    }

    void Update()
    {
        if (isInsideArea && flag_create_enemy && canRespawn)
        {
            flag_create_enemy = false;
            createEnemies();
            StartCoroutine(waitToEnableRespawn());
        }
    }

    IEnumerator waitToEnableRespawn()
    {
        yield return new WaitForSeconds(25f);
        canRespawn = true;
    }

        private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            isInsideArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInsideArea = false;
        }
    }

    private void createEnemies()
    {
        int n = numberOfEnemies;
        for (int i = 0; i < n; i++)
        {
            //Elijo un tipo de enemigo
            int type = Random.Range(0, enemyTypes.Count);

            //Elijo una posición aleatoria dentro del área
            float x = Random.Range(0, xPlacement);
            float z = Random.Range(0, zPlacement);
            float y = 0;
            if(enemyTypes[type].name == "Fairy")
            {
                y = 1f;
            }
            Vector3 nPos = new Vector3(downLeftCorner.x + x, transform.position.y + y, downLeftCorner.z + z);

            //Creo el clon y bajo el número de enemigos restantes por crear
            GameObject clone = Instantiate(enemyTypes[type], nPos, enemyTypes[type].transform.rotation);
            clone.SetActive(true);
            clone.GetComponent<Enemy>().spawner = thisSpawner;
            numberOfEnemies--;
        }
    }
}
