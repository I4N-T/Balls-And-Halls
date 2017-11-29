using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public Transform playerTransform;
    Rigidbody rb;

    //AUDIO
    public AudioClip rubberBandShootSoundClip;
    //public AudioClip playerHitSound;
    public AudioSource sfxSource;

    //MOVEMENT STUFF
    float movespeed;
    public float jumpForce;
    bool isGrounded;

    //SCENE 1
    //BALL STUFF
    public GameObject projectileObj;
    public BallScript ballScript;
    public MeshRenderer ballHudMeshRend;
    public Transform ballTrans;
    public float propulsionForce;

    public bool isHolding;

    public int playerHealth;

    Transform cameraDirection; //can delete?

    //SCENE 2
    bool isStartPositionSet;
    public static bool isKeyAcquired;

    void Start()
    {
        //set bools false (important for replay)
        isKeyAcquired = false;
        isStartPositionSet = false;

        sfxSource.clip = rubberBandShootSoundClip;

        DontDestroyOnLoad(transform.gameObject);

        playerTransform = this.transform;
        rb = this.GetComponent<Rigidbody>();

        movespeed = 10f;
        playerHealth = 3;

        //locks cursor on screen
        Cursor.lockState = CursorLockMode.Locked;

        ballHudMeshRend = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();
        ballHudMeshRend.enabled = false;
    }
	
	void Update ()
    {
        //Move using WASD
        MovementMethod();
        JumpMethod();

        //Check Health
        if (playerHealth <= 0)
        {
            //Game Over
            Destroy(gameObject);
            SceneManager.LoadScene(3);
        }

        //Shoot Projectile 
        if (!GameManager.isDodgeballGameWon)
        {
            BallThrowing();
        }

        //Initialize scene 2 stuff
        if (GameManager.currentScene.buildIndex == 2)
        {
            if (!isStartPositionSet)
            {
                //set start position
                transform.position = new Vector3(0, 2, 0);
                isStartPositionSet = true;

                //initialize health
                playerHealth = 100;

                //Disable ball mesh renderer 
                ballHudMeshRend = gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>();
                ballHudMeshRend.enabled = false;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
            
        }
    }

    void MovementMethod()
    {
        float translation = Input.GetAxis("Vertical") * movespeed;
        float strafe = Input.GetAxis("Horizontal") * movespeed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Translate(strafe, 0, translation);
    }

    void JumpMethod()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rb.AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void BallThrowing()
    {
        if (isHolding)
        {
            ballHudMeshRend.enabled = true;
            if (Input.GetMouseButton(0))
            {
                //increase propulsion force while fire button is held down
                propulsionForce += 0.25f;
                propulsionForce = Mathf.Clamp(propulsionForce, 0f, 16f);
            }
            if (Input.GetMouseButtonUp(0))
            {
                Vector3 forward = Camera.main.transform.forward;
                //Get ball script reference for the ball being held        
                ballScript = projectileObj.GetComponent<BallScript>();
                projectileObj.transform.parent = null;
                Rigidbody ballRB = projectileObj.GetComponent<Rigidbody>();
                ballRB.AddForce(forward * propulsionForce, ForceMode.Impulse);

                //mark ball as thrown
                ballScript.ballState = BallScript.BallFSM.Thrown;

                //add force in direction that camera is pointing
                propulsionForce = 0;
                //set ballscript ballState to Thrown
                isHolding = false;
            }

        }
        else if (!isHolding)
        {
            ballHudMeshRend.enabled = false;
        }
    }

    void Shoot()
    {
        //Play sound
        //sfxSource.clip = rubberBandShootSoundClip;
        sfxSource.Play();

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            //print(hit.transform.name);
            if (hit.transform.tag == "Enemy")
            {
                HallMonitorScript hmScript = hit.transform.GetComponent<HallMonitorScript>();
                Animator hmAnimator = hit.transform.GetComponent<Animator>();

                //Decrease health
                hmScript.health -= 1;
                //Animation; color flash
                if (hmScript.health >= 1)
                {
                    hmAnimator.SetTrigger("hitTrigger");
                }
                //sound
            }
            else if (hit.transform.tag == "Enemy2")
            {
                JockScript jockScript = hit.transform.GetComponent<JockScript>();
                Animator jockAnimator = hit.transform.GetComponent<Animator>();

                //Decrease health
                jockScript.health -= 1;
                //Animation; color flash
                if (jockScript.health >= 1)
                {
                    jockAnimator.SetTrigger("hitTrigger");
                }
                //sound
            }
            else if (hit.transform.tag == "Boss")
            {
                PrincipalScript principalScript = hit.transform.GetComponent<PrincipalScript>();
                Animator principalAnimator = hit.transform.GetComponent<Animator>();

                //Decrease health
                principalScript.health -= 1;
                //Animation; color flash
                if (principalScript.health >= 1)
                {
                    principalAnimator.SetTrigger("hitTrigger");
                }
                //sound
            }
        }
    }

    //COLLIDERS

     //Jumping
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.layer == 8)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.layer == 8)
        {
            isGrounded = false;
        }
    }

    //Door
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Door" && GameManager.isDodgeballGameWon && DialogueManager.isDialogueFinished && GameManager.currentScene.buildIndex == 1)
        {
            //disable scoreboard script
            ScoreBoardManager sbManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreBoardManager>();
            sbManager.enabled = false;
            //load next scene
            SceneManager.LoadScene(2);
        }
        //Principal's office door
        else if (col.gameObject.tag == "Door" && GameManager.currentScene.buildIndex == 2)
        {
            if (isKeyAcquired)
            {
                //transform position to be inside of principal's office. Maybe this will just be a whole new scene
                gameObject.transform.position = new Vector3(-50f, 1f, 82f);
            }
        }
    }
}
