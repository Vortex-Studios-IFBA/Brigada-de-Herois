using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float velRotacao = 2.5f;
    public float velMovimento = 3f;

    public AudioClip[] steps;
    AudioSource audios;
    bool deuPlay = false;
    private Quaternion originalRotation;
    private Quaternion cameraRotationY;
    public GameObject camm;

    //Animator anim;

    private void Start()
    {
        audios = GetComponent<AudioSource>();
        audios.clip = steps[0];
        //anim = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalRotation = transform.rotation;
        cameraRotationY = camm.transform.rotation;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera based on mouse input
        cameraRotationY *= Quaternion.Euler(-mouseY, mouseX * velRotacao, 0f);
        camm.transform.rotation = cameraRotationY;

        // Set the player's forward direction based on the camera's angle
        transform.rotation = Quaternion.Euler(originalRotation.eulerAngles.x, cameraRotationY.eulerAngles.y, originalRotation.eulerAngles.z);
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0.0f, vertical);

        if (movement != Vector3.zero)
        {
            if(!deuPlay)
            {
                audios.Play();
                deuPlay = true;
            }
            
            movement = transform.TransformDirection(movement);
            movement *= velMovimento;
            GetComponent<Rigidbody>().velocity = new Vector3(movement.x, GetComponent<Rigidbody>().velocity.y, movement.z);
        }
        else
        {
            audios.Stop();
            deuPlay = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("grass"))
        {
            audios.clip = steps[0];
            audios.volume = 0.1f;
        }
        if (collision.gameObject.CompareTag("ground"))
        {
            audios.clip = steps[1];
            audios.volume = 1;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        deuPlay = false;
    }
}
