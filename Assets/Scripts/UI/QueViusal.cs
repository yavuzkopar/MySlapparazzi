using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueViusal : MonoBehaviour
{
    CameraHolder cameraHolder;
    SsTaker ssTaker;
    void Start()
    {
        cameraHolder = FindObjectOfType<CameraHolder>();
        ssTaker= FindObjectOfType<SsTaker>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       transform.forward = cameraHolder.cameras[ssTaker.texIndex].transform.forward;
    }
}
