using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static Scene currentScene;
    public GameObject canvasObj;

    public int playerScore;
    public int opponentScore;

    public GameObject centerLine;

    public static bool isDodgeballGameWon;
    public bool isHallwaySceneBegin;

    //AUDIO
    public AudioSource musicSource;
    public AudioClip BandHThemeClip;
    public AudioClip hallwayTheme;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        DontDestroyOnLoad(canvasObj);

        isDodgeballGameWon = false;
        isHallwaySceneBegin = false;

        musicSource.clip = BandHThemeClip;
        musicSource.Play();
    }

    void Update()
    {
        //Keep track of current scene
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.buildIndex == 1)
        {
            if (isDodgeballGameWon)
            {
                //disable centerline invisible wall
                centerLine.SetActive(false);

                //Delete balls
                
            }
        }
        else if (currentScene.buildIndex == 2)
        {
            if (!isHallwaySceneBegin)
            {
                GameObject[] ballsArray = GameObject.FindGameObjectsWithTag("Ball");
                for (int i = 0; i < ballsArray.Length; i++)
                {
                    Destroy(ballsArray[i]);
                }
                isHallwaySceneBegin = true;

                //Change music
                musicSource.clip = hallwayTheme;
                musicSource.Play();
            }
        }
        else if (currentScene.buildIndex == 3 || currentScene.buildIndex == 4)
        {
            
            Destroy(canvasObj);
            Destroy(gameObject);
        }

        
    }

}
