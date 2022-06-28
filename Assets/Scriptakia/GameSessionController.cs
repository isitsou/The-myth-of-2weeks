using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSessionController : MonoBehaviour
{


    //public vars
    [SerializeField] int playerLives = 3;
    [SerializeField] int diamondCounter = 0;

    //cached refs
    public Image[] hearts;
    public Text diamondTextCounter;

    [HideInInspector] public UnityEvent<bool> DiamondsAdjusted;

    public static GameSessionController instance = null;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //Awake is always called before any Start functions AND its called twice if you have open the edit-scene, on full screen its called once
    private void Awake()
    {
        /*Create Singleton pattern for the gameObject GameSession          
        (info:"In software engineering, the singleton pattern is a software 
        design pattern that restricts the instantiation of a class to one "single" instance. 
        This is useful when exactly one object is needed to coordinate actions across the system. ") */

        if (instance != null && instance != this)
        {
            Destroy(gameObject);//when we load a scene if there is an existing gameSessionController from before then dont make a new one(we want to keep the data intact for this session)
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);//when we load a scene and we know that there is only one gameSession object then keep it intact

    }

    // Start is called before the first frame update
    void Start()
    {
        diamondTextCounter.text = diamondCounter.ToString();
        DiamondsAdjusted.AddListener(DiamondsUIHandler);
    }

    public void PlayersDeathController()
    {
        if (playerLives > 1)
        {
            TakeOneLife();
        }
        else
        {
            playerLives--;
            hearts[playerLives].enabled = false;
            ResetGameSession();
        }
    }
    public void ResetGameSession()
    {
        //GOTO main menu scene
        Destroy(FindObjectOfType<ScenePersistController>().gameObject);
        StartCoroutine(WaitForSceneLoad(0));        
    }

    private void ResetUIs()
    {
        playerLives = 3;
        for (int i = 0; i < playerLives; i++)
        {
            hearts[i].enabled = true;
        }
        while (diamondCounter > 0)
        {
            SubDiamonds(1);
        }
    }

    public void AddDiamond()
    {
        diamondCounter++;
        DiamondsAdjusted?.Invoke(diamondCounter >= 3);
    }

    public void SubDiamonds(int usedDiamonds)
    {
        diamondCounter -= usedDiamonds;
        DiamondsAdjusted?.Invoke(diamondCounter >= 3);
    }

    private void DiamondsUIHandler(bool diamondsAreEnough)
    {       
        if (diamondsAreEnough)
        {
            diamondTextCounter.text = diamondCounter.ToString() + "\nHammer Time!";
        }
        else
        {
            diamondTextCounter.text = diamondCounter.ToString();
        }
    }
    private void TakeOneLife()
    {
        //take one life from players health and reload from the start the same scene        
        playerLives--;
        hearts[playerLives].enabled = false;
        StartCoroutine(WaitForSceneLoad(SceneManager.GetActiveScene().buildIndex));
    }
    // load the same scene with delay
    private IEnumerator WaitForSceneLoad(int sceneIndex)
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene sceneLoaded, LoadSceneMode loadSceneMode)
    {
        DiamondsAdjusted.AddListener(FindObjectOfType<Player>().CheckDiamonds);

        if (sceneLoaded.buildIndex == 0) ResetUIs();        
        DiamondsAdjusted?.Invoke(diamondCounter >= 3);
    }
}
