using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSessionController : MonoBehaviour
{


    //public vars
    [SerializeField] int playerLives = 3;
    [SerializeField] int diamondCounter=0;

    //cached refs
    public Image[] hearts;
    public Text diamondTextCounter;



    //Awake is always called before any Start functions AND its called twice if you have open the edit-scene, on full screen its called once
    private void Awake()
    {
        /*Create Singleton pattern for the gameObject GameSession          
        (info:"In software engineering, the singleton pattern is a software 
        design pattern that restricts the instantiation of a class to one "single" instance. 
        This is useful when exactly one object is needed to coordinate actions across the system. ") */

        int gameSessionCounter = FindObjectsOfType<GameSessionController>().Length; //number of GameSessionController that exist
        if (gameSessionCounter > 1)
        {
            Destroy(gameObject);//when we load a scene if there is an existing gameSessionController from before then dont make a new one(we want to keep the data intact for this session)
        }
        else
        {
            DontDestroyOnLoad(gameObject);//when we load a scene and we know that there is only one gameSession object then keep it intact
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        diamondTextCounter.text = diamondCounter.ToString();
    }

    public void PlayersDeathController()
    {
        if (playerLives > 1)
        {
            TakeOneLife();
        }
        else
        {
            ResetGameSession();
        }
    }
    public void ResetGameSession()
    {
        //GOTO main menu scene
        Destroy(FindObjectOfType<ScenePersistController>().gameObject);
        StartCoroutine(WaitForSceneLoad());
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void AddDiamond()
    {
        diamondCounter++;
        diamondTextCounter.text = diamondCounter.ToString();
    }
    private void TakeOneLife()
    {
        //take one life from players health and reload from the start the same scene        
        playerLives--;
        hearts[playerLives].enabled = false;
        StartCoroutine(WaitForSceneLoad()); 
    }
    // load the same scene with delay
    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


}
