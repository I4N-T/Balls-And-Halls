using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{

    public Button playBtn;
    public AudioSource musicSource;
    public AudioClip BandHTheme;

    void Awake()
    {
        //unlocks cursor on screen
        Cursor.lockState = CursorLockMode.None;

        musicSource.clip = BandHTheme;
        playBtn.onClick.AddListener(PlayBtnAction);
        musicSource.Play();
    }

    public void PlayBtnAction()
    {
        SceneManager.LoadScene(1);
    }
}
