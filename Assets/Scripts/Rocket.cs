using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // states 
    Rigidbody rigidBody;
    AudioSource audiosource;

    // adjustable states
    [SerializeField] float thrustScale = 100f;
    [SerializeField] float rotationScale = 100f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrustInput();
        RotateInput();
    }

    // Process key presses every frame in Update()
    private void ThrustInput() 
    {
        float thrustMagnitude = thrustScale * Time.deltaTime;
        if ( Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) ) {
            print("Thrusting");
            rigidBody.AddRelativeForce(thrustScale*Vector3.up);
        } 
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) {
            audiosource.Play();
        } else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) {
            audiosource.Stop();
        }
    }
    private void RotateInput() 
    {
        rigidBody.freezeRotation = true;
        float rotationMagnitude = rotationScale * Time.deltaTime;

        if ( Input.GetKey(KeyCode.A) ) {
            print("Rotate Left");
            // Vector3.forward is the Z axis
            // lefthanded coordinate system
            transform.Rotate(rotationMagnitude*Vector3.forward);
        }
        else if ( Input.GetKey(KeyCode.D) ) {
            print("Rotate Right");
            transform.Rotate(rotationMagnitude*(-Vector3.forward));
        }
    }
}
