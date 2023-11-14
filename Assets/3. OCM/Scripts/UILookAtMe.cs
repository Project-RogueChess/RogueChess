using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAtMe : MonoBehaviour
{
    public Camera camera;

    private void Awake()
    {
        camera = FindObjectOfType<Camera>();
    }
    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
