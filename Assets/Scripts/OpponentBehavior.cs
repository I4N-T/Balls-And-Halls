using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OpponentBehavior : MonoBehaviour {

    //References to other stuff
    public GameObject playerObj;
    public GameObject projectileObj;
    BallScript ballScript;

    //References to this object's stuff
    Rigidbody rb;

    public int OpponentHealth;


    public List<GameObject> ballListAll = new List<GameObject>();
    public List<GameObject> ballListCourt = new List<GameObject>();

    //behavioral
    public bool isHoldingBall;
    bool isBallAvailable;
    bool isLocationDecided;
    Vector3 targetPos;


	// Use this for initialization
	void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        OpponentHealth = 3;
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        //Rotate to face toward camera
        FaceCamera();

        //Health check
        if (OpponentHealth <= 0)
        {
            GameManager.isDodgeballGameWon = true;
            //playerHealth = 3?

            //A voice comes over the PA system... 
            //Hey you! Report to the principal's office. IMMEDIATELY!!!!
            //Head through the door under the scoreboard to enter the school hallway.
        }

        //ONLY DO THIS STUFF IF THE GAME IS HAPPENING
        if (!GameManager.isDodgeballGameWon)
        {


            //if not holding ball, pick up ball if available. Else, move back and forth trying to dodge
            GetBallCourtList();
            if (ballListCourt.Count <= 0)
            {
                isBallAvailable = false;
            }
            else if (ballListCourt.Count > 0)
            {
                isBallAvailable = true;
            }

            //Go to ball to pick up
            if (isBallAvailable && !isHoldingBall)
            {
                float mag1 = 0;
                float mag2 = 9999f;

                //Find closest available ball and go to it
                for (int i = 0; i < ballListCourt.Count; i++)
                {
                    GameObject ball = ballListCourt[i];
                    mag1 = Vector3.Distance(transform.position, ball.transform.position);
                    if (mag1 < mag2)
                    {
                        mag2 = mag1;
                        targetPos = ball.transform.position;
                    }
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
            }

            //If holding ball, move around a bit then throw it
            if (isHoldingBall)
            {

                if (!isLocationDecided)
                {
                    //randomly pick location to go to before throwing the ball
                    float xRand = Random.Range(-8f, 8f);
                    float zRand = Random.Range(2f, 17f);
                    targetPos = new Vector3(xRand, 1f, zRand);
                    isLocationDecided = true;
                }
                //actually move to the spot
                else if (isLocationDecided)
                {
                    if (transform.position != targetPos)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetPos, 5f * Time.deltaTime);
                    }
                    else if (transform.position == targetPos)
                    {
                        //throw ball
                        ballScript = projectileObj.GetComponent<BallScript>();
                        projectileObj.transform.parent = null;
                        Rigidbody theRb = projectileObj.GetComponent<Rigidbody>();
                        theRb.AddForce(transform.forward * 15f, ForceMode.Impulse);
                        isLocationDecided = false;
                        isHoldingBall = false;
                        ballScript.ballState = BallScript.BallFSM.Thrown;

                    }

                }

            }
        }


    }

    void FaceCamera()
    {
        Vector3 targetPosition = new Vector3(playerObj.transform.position.x, transform.position.y
            , playerObj.transform.position.z);
        transform.LookAt(targetPosition);
    }

    void GetBallCourtList()
    {
        ballListCourt.Clear();
        for (int i = 0; i < ballListAll.Count; i++)
        {
            BallScript ballScript = ballListAll[i].GetComponent<BallScript>();
            if (!ballScript.isPlayerCourt())
            {
                if (ballScript.ballState == BallScript.BallFSM.PickupReady)
                {
                    ballListCourt.Add(ballListAll[i]);
                }      
            }   
        }       
    }
}
