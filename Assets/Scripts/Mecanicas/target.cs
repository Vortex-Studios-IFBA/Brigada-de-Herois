using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip[] sfx;
    AudioSource open;
    public float range = 10f;
    public bool locked = true;
    Animator door;
    Camera cam;
    public bool isOpening = false, inRange = false;
    void Start()
    {
        open = GetComponent<AudioSource>();
        door = GetComponent<Animator>();
        open.clip = sfx[0];
        cam = Camera.main;
        if(gameObject.name == "Lab1" || gameObject.name == "Lab2")
        {
            locked = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!locked)
        {if(inRange)
        {if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            
            StartCoroutine(OpenDoor());
        }}}
        else
        {
            if(inRange)
            {
                if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton0))
                {
                    open.volume = 0.5f;
                    open.Play();
                    print("Tracado");
                }
            }
        }
    }
    IEnumerator OpenDoor()
    {
        if(isOpening)
        yield break;

        isOpening = true;
        open.clip = sfx[1];
        open.volume = 1;
        open.Play();
        yield return new WaitForSeconds(0.5f);
        door.Play("DoorOpen");
        yield return new WaitForSeconds(5f);
        //door.Play("New State");
        //isOpening = false;
    }
    void OnTriggerEnter(Collider col)
    {
        inRange = true;
    }
    void OnTriggerExit(Collider col)
    {
        inRange = false;
    }
}
