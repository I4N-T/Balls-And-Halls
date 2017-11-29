using UnityEngine;
using System.Collections;

public class JockScript : MonoBehaviour {

    //References to other stuff
    public GameObject playerObj;
    public GameObject ballObj;
    public GameObject ballThrowObj;

    public int health;

    float distance;
    Vector3 startPos;
    public Vector3 trajectory;

    bool isPlayerSeen;
    bool isGoingRight;
    bool isCRStarted;
    public bool isZPatrol;

    //AUDIO
    public AudioClip deathSoundClip;
    public AudioSource sfxSource;

    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        health = 3;
        startPos = transform.position;

        //Audio
        sfxSource.clip = deathSoundClip;
    }

    void Update()
    {
        //Rotate to face toward camera
        FaceCamera();

        //Movement
        Patrol();

        //If within range of player, attack
        if (isPlayerSeen)
        {
            if (!isCRStarted)
            {
                StartCoroutine(ThrowBall());
            }
            
        }

        LookForPlayer();

        if (health <= 0)
        {
            if (!sfxSource.isPlaying)
            {
                sfxSource.Play();
            }
            Animator jockAnimator = gameObject.GetComponent<Animator>();
            jockAnimator.SetTrigger("deathTrigger");
            Destroy(gameObject, .5f);
        }

    }

    void FaceCamera()
    {
        Vector3 targetPosition = new Vector3(playerObj.transform.position.x, transform.position.y
            , playerObj.transform.position.z);
        transform.LookAt(targetPosition);
    }

    void Patrol()
    {
        if (isZPatrol)
        {
            //Patrol in z direction
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * 3f, 7) + (startPos.z - 3.5f));
        }
        else if (!isZPatrol)
        {
            //Patrol in x direction
            transform.position = new Vector3(Mathf.PingPong(Time.time * 3f, 7) + (startPos.x - 3.5f), transform.position.y, transform.position.z);
        }
        
    }

    void LookForPlayer()
    {
        //if raycast hits AND within range then go after player
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                isPlayerSeen = true;
            }
            if (hit.transform.tag != "Player")
            {
                isPlayerSeen = false;
            }
        }
    }

    IEnumerator ThrowBall()
    {
        while (isPlayerSeen)
        {
            
            trajectory = transform.forward;
            isCRStarted = true;
            ballThrowObj = Instantiate(ballObj, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            HallwayBallScript ballScript = ballThrowObj.GetComponent < HallwayBallScript >();
            ballScript.trajectory = trajectory;
            yield return new WaitForSeconds(3f);
        }

        isCRStarted = false;

    }
}
