using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

    public float speed = 50f;

    //AUDIO
    public AudioClip pickupSoundClip;
    public AudioSource sfxSource;


    void Start()
    {
        sfxSource.clip = pickupSoundClip;
    }
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            PlayerController.isKeyAcquired = true;
            sfxSource.Play();

            Destroy(gameObject, .5f);
        }
    }
}
