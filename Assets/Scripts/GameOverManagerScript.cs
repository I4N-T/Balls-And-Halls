using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManagerScript : MonoBehaviour {

    public Button replayBtn;

    void Awake()
    {
        //unlocks cursor on screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        replayBtn.onClick.AddListener(replayBtnAction);
    }

    void replayBtnAction()
    {
        SceneManager.LoadScene(0);
    }
}
