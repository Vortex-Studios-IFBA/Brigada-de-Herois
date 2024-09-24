using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRotation : MonoBehaviour
{
    private float velRotacao = 2.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        transform.rotation *= Quaternion.Euler(-mouseY * velRotacao, 0f, 0f);
    }
}
