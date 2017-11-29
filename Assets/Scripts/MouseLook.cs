using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {

    Vector2 mouseLook;
    Vector2 smoothVector;
    Vector2 md;
    public float sensitivity;
    public float smoothing;

    GameObject charObj;

    void Start()
    {
        charObj = this.transform.parent.gameObject;

        sensitivity = 2f;
        smoothing = 2f;
    }

    void Update()
    {
        md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothVector.x = Mathf.Lerp(smoothVector.x, md.x, 1f / smoothing);
        smoothVector.y = Mathf.Lerp(smoothVector.y, md.y, 1f / smoothing);
        mouseLook += smoothVector;

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        charObj.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, charObj.transform.up);
    }

}
