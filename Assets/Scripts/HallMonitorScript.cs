using UnityEngine;
using System.Collections;

public class HallMonitorScript : MonoBehaviour {

    //References to other stuff
    public GameObject playerObj;

    public int health;

    float distance;

    bool isPlayerSeen;
    bool isDamaging;
    bool isCRStarted;

    //AUDIO
    public AudioClip deathSoundClip;
    public AudioClip playerHitSound;
    public AudioSource sfxSource;

    void Start ()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        health = 3;
        
        //Audio
        sfxSource.clip = deathSoundClip;

        //StartCoroutine(DamageOverTime());
        //isCRStarted = true;
    }
	
	void Update ()
    {
        //Restart damage cr if need to
        if (isDamaging)
        {
            if (!isCRStarted)
            {
                StartCoroutine(DamageOverTime());
                isCRStarted = true;
            }
        }
        

        //Rotate to face toward camera
        FaceCamera();

        //If within range of player, attack
        if (isPlayerSeen)
        {
            GoGetPlayer();
        }

        LookForPlayer();

        if (health <= 0)
        {
            if (!sfxSource.isPlaying)
            {
                sfxSource.clip = deathSoundClip;
                sfxSource.Play();
            }
            Animator hmAnimator = gameObject.GetComponent<Animator>();
            hmAnimator.SetTrigger("deathTrigger");
            Destroy(gameObject, .5f);   
        }

    }

    void FaceCamera()
    {
        Vector3 targetPosition = new Vector3(playerObj.transform.position.x, transform.position.y
            , playerObj.transform.position.z);
        transform.LookAt(targetPosition);
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
        }
    }

    void GoGetPlayer()
    {
        distance = Vector3.Distance(transform.position, playerObj.transform.position);
        if (distance <= 25f)
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerObj.transform.position.x, 1.5f, playerObj.transform.position.z), 3f * Time.deltaTime);
    }

    IEnumerator DamageOverTime()
    {
        PlayerController playerScript = playerObj.GetComponent<PlayerController>();
        while (isDamaging)
        {
            playerScript.playerHealth -= 10;
            //grunt
            sfxSource.clip = playerHitSound;
            sfxSource.Play();
            yield return new WaitForSeconds(2f);
        }
        isCRStarted = false;
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            isDamaging = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            isDamaging = false;
            //stop coroutine?
        }
    }
}
