using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthTextScript : MonoBehaviour {

    public Text healthTxt;
    GameObject playerObj;
    PlayerController playerScript;

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerObj.GetComponent<PlayerController>();
    }

    void Update()
    {
        healthTxt.text = playerScript.playerHealth.ToString();

        //Only show health in hallway scene
        if (GameManager.currentScene.buildIndex == 2)
        {
            healthTxt.enabled = true;
        }
        else if (GameManager.currentScene.buildIndex != 2)
        {
            healthTxt.enabled = false;
        }
    }
	
}
