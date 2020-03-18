using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketReview : MonoBehaviour {

    enum State {Alive, Dying, Transcending};
    State state = State.Alive;

    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float thrust = 10f;
    [SerializeField] float loadTime = 2f;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return;  }

        switch (collision.gameObject.tag)
        {
            
            case "Friendly":
                Debug.Log("Safe Space");
                break;

            case "Goal":
                state = State.Transcending;
                Invoke("LoadNextScene", loadTime);
                break;

            default:
                state = State.Dying;
                Invoke("LoadBeginning", loadTime);
                break;
        }
    }

    private void LoadBeginning()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);  //TODO - allow for >2 levels
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * thrust);
            if (audioSource.isPlaying == false)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {

        rb.freezeRotation = true;
        float rotationSpeed = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rb.freezeRotation = false;
    }
}
