using UnityEngine;
using System.Collections;

public class HallwayBallScript : MonoBehaviour {

    public Vector3 trajectory;
    public AudioSource sfxSource;
    public AudioClip playerHitSound;
	
	void Start()
    {
        //sfxSource = gameObject.transform.GetComponent<AudioSource>();
        sfxSource.clip = playerHitSound;
    }

	void Update ()
    {
        transform.Translate(trajectory * Time.deltaTime * 8f);

    }

   void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Enemy" && col.tag != "Enemy2" && col.tag != "Boss")
        {
            if (col.tag == "Player")
            {
                PlayerController playerScript = col.gameObject.GetComponent<PlayerController>();
                playerScript.playerHealth -= 15;
                sfxSource.Play();
                print("test ok");
            }
            Destroy(gameObject, 0.5f);

        }

        
    }
}
