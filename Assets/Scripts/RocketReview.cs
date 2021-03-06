﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketReview : MonoBehaviour {

    enum State {Alive, Dying, Transcending, Immortal};
    State state = State.Alive;
    bool collisionsDisabled = false;

    Rigidbody rb;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 10f;
    [SerializeField] float thrust = 10f;
    [SerializeField] float loadDelayTime = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip startSound;
    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] ParticleSystem goalParticles;
    [SerializeField] ParticleSystem deathParticles;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(state == State.Alive || state == State.Immortal)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled)
        {
            return;
        }
        
        switch (collision.gameObject.tag)
        {
            
            case "Friendly":
                Debug.Log("Safe Space");
                break;

            case "Goal":
                StartGoalSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartGoalSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        thrustParticles.Stop();
        audioSource.PlayOneShot(startSound);
        goalParticles.Play();
        Invoke("LoadNextScene", loadDelayTime);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        thrustParticles.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadBeginning", loadDelayTime);
    }

    private void LoadBeginning()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex >= SceneManager.sceneCountInBuildSettings) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);  
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            thrustParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rb.AddRelativeForce(Vector3.up * thrust);
        thrustParticles.Play();
        if (audioSource.isPlaying == false)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
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

    private void RespondToLevelChangeInput()
    {
        if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene(1);
        }
    }

    private void RespondToCollisionInput()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (state != State.Immortal)
            {
                state = State.Immortal;
            }
            else
            {
                state = State.Alive;
            }
        }
    }

}
