using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DataShowController : MonoBehaviour
{
    public VideoPlayer video;
    void Start()
    {
        video = GameObject.Find("TelaSlide").GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         video.Play();
    //     }
    // }
}
