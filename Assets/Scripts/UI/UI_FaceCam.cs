using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FaceCam : MonoBehaviour
{
    Camera mainCam;
    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
    }
    void LateUpdate()
    {
        //ui fica paralela a camera
        transform.LookAt(transform.position - Camera.main.transform.rotation * Vector3.back, 
            mainCam.transform.rotation * Vector3.up);

        //ui olha para a camera (precisa consertar direção)
        //transform.LookAt(mainCam.transform.position, Vector3.up);
    }
}
