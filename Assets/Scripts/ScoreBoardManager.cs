using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour {

    GameManager gMScript;

    public Text playerScoreTxt;
    public Text OppScoreTxt;

    void Start()
    {
        gMScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        playerScoreTxt.text = gMScript.playerScore.ToString();
        OppScoreTxt.text = gMScript.opponentScore.ToString();
    }

}
