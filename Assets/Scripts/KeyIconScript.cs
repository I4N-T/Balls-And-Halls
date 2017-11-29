using UnityEngine;
using System.Collections;

public class KeyIconScript : MonoBehaviour {

    public GameObject keyIconObj;

	void Update()
    {
        if (PlayerController.isKeyAcquired)
        {
            keyIconObj.SetActive(true);
        }
    }
}
