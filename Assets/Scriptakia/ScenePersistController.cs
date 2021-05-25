using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersistController : MonoBehaviour
{

    //vars
    private int startingSceneIndex;
    private int scenePersistStuffCounter;
    private int currentSceneIndex;



    //Awake is always called before any Start functions AND its called twice if you have open the edit-scene, on full screen its called once
    private void Awake()
    {             
        scenePersistStuffCounter = FindObjectsOfType<ScenePersistController>().Length;
        if(scenePersistStuffCounter > 1)
        {      
                //Debug.Log("destroy on awake2: " + gameObject.name);
                Destroy(gameObject);                 
        }
        else 
        {
            //Debug.Log("dont destroy on awake: " + gameObject.name);
            DontDestroyOnLoad(gameObject);
        }
        
    }
    /*
   private void Start()
   {
       startingSceneIndex = SceneManager.GetActiveScene().buildIndex; 
       Debug.Log("on Start... startingSceneIndex " + startingSceneIndex);
   }

   private void Update()
   {
       //destruction of ScenePersistStuff of previous scene
       currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       if (currentSceneIndex != startingSceneIndex)
       {
           Debug.Log("currentSceneIndex: "+SceneManager.GetActiveScene().buildIndex +"  StartingSceneIndex: "+startingSceneIndex);
           Debug.Log("Destroy on update:" + gameObject.name);
           Destroy(gameObject);
       }
   }*/
}
