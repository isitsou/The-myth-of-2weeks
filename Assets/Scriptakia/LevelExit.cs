using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //public vars
    [SerializeField] float levelLoadDelay = 0.5f;
    //[SerializeField] float slowMoFactor = 1f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Animator ANIM = null;
        //activate the trigger for exiting in Player
        ANIM = collision.gameObject.GetComponentInParent<Animator>();
        ANIM.SetTrigger("IsExiting");


        StartCoroutine(LoadNextLevel());
    }
    IEnumerator LoadNextLevel()
    {
        //Slow motion effect
        //Time.timeScale = slowMoFactor;

        //wait for levelLoadDelay seconds until you load the next scene
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        //turn off slow motion
        //Time.Scale = 1f;


        //get the number(index) of the current scene
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //load next scene
        Destroy(FindObjectOfType<ScenePersistController>().gameObject);
        Debug.Log("destroy on exit");
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
