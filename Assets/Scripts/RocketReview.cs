using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketReview : MonoBehaviour {

    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float thrust = 10f;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Thrust();
        Rotate();  
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");
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
