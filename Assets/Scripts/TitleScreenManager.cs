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
        musicSource.clip = BandHTheme;
        playBtn.onClick.AddListener(PlayBtnAction);
        musicSource.Play();
    }

    public void PlayBtnAction()
    {
        SceneManager.LoadScene(1);
    }
}
