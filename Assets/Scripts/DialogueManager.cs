using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    //GameManager gmScript;

    public GameObject diagBox;
    public Text diagTxt;

    public bool dialogActive;
    public int textCount;

    public static bool isDialogueFinished;

    void Start()
    {
        //DontDestroyOnLoad(transform.gameObject);
        textCount = 0;

        isDialogueFinished = false;
    }

    void Update()
    {
        if (dialogActive && Input.GetKeyDown("e"))
        {
            diagBox.SetActive(false);
            textCount += 1;
            dialogActive = false;
        }

        //Post dodgeball victory message sequence
        if (GameManager.isDodgeballGameWon && textCount == 0)
        {
            //play victory jingle
            ShowDialogue("You win! Congratulations!");
        }
        else if (GameManager.isDodgeballGameWon && textCount == 1)
        {
            ShowDialogue("A voice comes over the PA system...");
        }
        else if (GameManager.isDodgeballGameWon && textCount == 2)
        {
            ShowDialogue("\"Hey you! Report to the principal's office. IMMEDIATELY!!!\"");
        }
        else if (GameManager.isDodgeballGameWon && textCount == 3)
        {
            ShowDialogue("Head through the door under the scoreboard to enter the school hallway.");
        }
        else if (GameManager.isDodgeballGameWon && textCount == 4)
        {
            isDialogueFinished = true;
        }

        //SCENE 2
        if (GameManager.currentScene.buildIndex == 2 && textCount == 4)
        {
            ShowDialogue("Left click to shoot rubber bands at your enemies. Enter the principal's office to win.");
        }

        
    }

    public void ShowDialogue(string textToDisplay)
    {
        dialogActive = true;
        diagBox.SetActive(true);
        diagTxt.text = textToDisplay;
    }

}
