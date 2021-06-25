using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class SFX_cutscene : MonoBehaviour
{
    public GameObject camara;
    public GameObject cutscene;
    public GameObject hud;
    public GameObject enemyBar;
    public GameObject playerStats;
    public GameObject inventary;
    public GameObject dialogos;

    public GameObject skipPanel;
    
    PostProcessVolume m_Volume;
    Vignette m_Vignette;
    void Start()
    {
        // Create an instance of a vignette
        m_Vignette = ScriptableObject.CreateInstance<Vignette>();
        m_Vignette.enabled.Override(true);
        //m_Vignette.intensity.Override(1f);
        // Use the QuickVolume method to create a volume with a priority of 100, and assign the vignette to this volume
        m_Volume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
        
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad >= 20f || Input.GetKeyDown(GameConstants.key_interact))
        {
            Debug.Log("Termina la cinematica");
            m_Vignette.enabled.Override(false);
            cutscene.SetActive(false);
            //dialogos.SetActive(false);
            Destroy(dialogos);
            camara.SetActive(true);
            inventary.SetActive(true);
            hud.SetActive(true);
            enemyBar.SetActive(true);
            playerStats.SetActive(true);
            skipPanel.SetActive(false);
        }
        // Change vignette intensity using a sinus curve
        //m_Vignette.intensity.value = Mathf.Sin(Time.realtimeSinceStartup);
    }
    void OnDestroy()
    {
        RuntimeUtilities.DestroyVolume(m_Volume, true, true);
    }
}