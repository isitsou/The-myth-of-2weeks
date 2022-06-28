using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //used on MainMenu
    public void Load_1stLevel()
    {
        Destroy(FindObjectOfType<ScenePersistController>().gameObject);
        Debug.Log("D");
        SceneManager.LoadScene(1);
    }
    
    
    //used on SuccessScreen
    public void Load_0stLevel()
    {
        //restore health to full and load the main menu scene
        FindObjectOfType<GameSessionController>().ResetGameSession();
    }
}
