using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    public Rigidbody2D rigi;
    public float vel;
    public float jump;

    public bool isGrounded;
    public bool dJump;
    public int respawn;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

   
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        transform.position += movement * Time.deltaTime * vel;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigi.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D groundColi) 
    {
        if(groundColi.gameObject.layer == 8)
        {
            isGrounded = true;
        }

        if(groundColi.gameObject.layer == 9)
        {
            SceneManager.UnloadSceneAsync(1);
            //SceneManager.LoadScene(respawn);
        }
        
        if(groundColi.gameObject.tag == "spike")
        {
            //Destroy(gameObject);
            SceneManager.UnloadSceneAsync(1);
            //SceneManager.LoadScene(respawn);
        }
    }

    void OnCollisionExit2D(Collision2D groundColi) 
    {
        if(groundColi.gameObject.layer == 8)
        {
        isGrounded = false;
        Debug.Log("não ta no chão");
        }
    }

}
