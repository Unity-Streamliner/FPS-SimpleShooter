using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform target;

    private float startFOV;
    private float targetFOV;
    public float zoomSpeed = 1f;

    public Camera theCamera;

    void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        startFOV = theCamera.fieldOfView;
        targetFOV = startFOV;
    }

    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;

        theCamera.fieldOfView = Mathf.Lerp(theCamera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void zoomIn(float newZoom) 
    {
        targetFOV = newZoom;
    }

    public void zoomOut() 
    {
        targetFOV = startFOV;
    }
}
