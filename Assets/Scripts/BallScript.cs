using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

    PlayerController playerControllerScript;
    OpponentBehavior opponentScript;
    GameManager gMScript;

    Rigidbody rb;
    MeshRenderer meshRend;
    //bool isPlayerCourt;

    public enum BallFSM
    {
        PickupReady,
        Held,
        Thrown,
        HeldOpponent
    }

    //Default State
    public BallFSM ballState = BallFSM.PickupReady;

    void Start()
    {
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        opponentScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<OpponentBehavior>();
        gMScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        rb = gameObject.GetComponent<Rigidbody>();
        meshRend = gameObject.GetComponent<MeshRenderer>();
    }


    

    void Update()
    {
        //Determine which side of court ball is on
        isPlayerCourt();

        if (playerControllerScript != null)
        {
            //Hold ball in view in front of player char
            if (ballState == BallFSM.Held)
            {
                //playerControllerScript.ballHudMeshRend.enabled = true;
                meshRend.enabled = false;
                gameObject.transform.localPosition = new Vector3(0f, 1f, 0.5f);
                gameObject.transform.rotation = Quaternion.identity;
            }
        }

        if (ballState == BallFSM.HeldOpponent)
        {
            meshRend.material.color = Color.white;
            gameObject.transform.localPosition = new Vector3(0f, 1f, 0.7f);
            gameObject.transform.rotation = Quaternion.identity;
        }

        //If ball is not moving, then it can be picked up (isInPlay = false)
        if (ballState == BallFSM.Thrown)
        {
            gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //set color to regular, enable renderer, check speed
            meshRend.material.color = Color.white;
            meshRend.enabled = true;
            CheckBallSpeed();
        }

        if (ballState == BallFSM.PickupReady)
        {
            meshRend.material.color = Color.blue;
        }
    }

    //Determines which side of court ball is on
    public bool isPlayerCourt()
    {
        if (transform.position.z < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (ballState == BallFSM.PickupReady)
            {
                if (!playerControllerScript.isHolding && !GameManager.isDodgeballGameWon)
                {
                    //print("bingo");
                    gameObject.transform.parent = col.gameObject.transform;
                    //playerControllerScript = col.gameObject.GetComponent<PlayerController>();
                    playerControllerScript.isHolding = true;
                    playerControllerScript.projectileObj = this.gameObject;
                    playerControllerScript.ballTrans = this.transform;
                    ballState = BallFSM.Held;
                }
            }
            else if (ballState == BallFSM.Thrown)
            {
                playerControllerScript.playerHealth -= 1;
                //Opponent Score + 1
                gMScript.opponentScore += 1;
            }
        }

        if (col.gameObject.tag == "Enemy")
        {
            if (ballState == BallFSM.PickupReady)
            {
                if (!opponentScript.isHoldingBall)
                {
                    //print("bingo");
                    gameObject.transform.parent = col.gameObject.transform;
                    gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    opponentScript = col.gameObject.GetComponent<OpponentBehavior>();
                    opponentScript.isHoldingBall = true;
                    opponentScript.projectileObj = this.gameObject;
                    ballState = BallFSM.HeldOpponent;
                }
            }
            else if (ballState == BallFSM.Thrown)
            {
                opponentScript.OpponentHealth -= 1;
                //Player Score + 1
                gMScript.playerScore += 1;
            }
        }
    }

    void CheckBallSpeed()
    {
        float speed;
        speed = rb.velocity.magnitude;
        if (speed < 0.5f)
        {
            ballState = BallFSM.PickupReady;
        }
    }
}
