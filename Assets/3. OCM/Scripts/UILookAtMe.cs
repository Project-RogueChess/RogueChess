using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtMe : MonoBehaviour
{
    public Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }
    void Update()
    {
        transform.forward = camera.transform.forward;
        //transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
