using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    // states 
    Rigidbody rigidBody;
    AudioSource audiosource;
    enum GameState{Alive, Dying, Transcending};
    GameState gameState = GameState.Alive;

    // adjustable states
    [SerializeField] float thrustScale = 100f;
    [SerializeField] float rotationScale = 100f;
    [SerializeField] float DyingSeconds = 1.5f;

    // audio references
    [SerializeField] AudioClip MainEngineAudio;
    [SerializeField] AudioClip DeathExplosionAudio;
    [SerializeField] AudioClip SuccessAudio;

    [SerializeField] ParticleSystem MainEngineParticle;
    [SerializeField] ParticleSystem DeathExplosionParticle;
    [SerializeField] ParticleSystem SuccessParticle;

    bool booster_L_landed = false;
    bool booster_R_landed = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.Alive) {
            ThrustInput();
            RotateInput();
        }
        
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
            audiosource.PlayOneShot(MainEngineAudio);
            MainEngineParticle.Play();
        } else if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W)) {
            audiosource.Stop();
            MainEngineParticle.Stop();
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
        rigidBody.freezeRotation = false;
    }

    // collision
    void OnCollisionEnter(Collision collision) {
        print("Collided using: " + collision.contacts[0].thisCollider.name);
        if (gameState == GameState.Alive) {
            switch(collision.gameObject.tag) {
                case "Friendly":
                    print("Collided Friendly");
                    break;
                case "Landingpad":       
                    CollideLandingpad();
                    break;
                default:
                    CollideDead();
                    break;
            }
        }
        // check landing legs - if they collided with landing pad
        if (collision.gameObject.tag == "Landingpad") {
            string rocketPart = collision.contacts[0].thisCollider.name;

            if (rocketPart == "Rocket_boosterL") {
                booster_L_landed = true;
            } else if (rocketPart == "Rocket_boosterR") {
                booster_R_landed = true;
            }
        }
    }

    private void CollideDead() {
        // die leads restarting the game 
        print("Death code runs");
        gameState = GameState.Dying;
        rigidBody.constraints = RigidbodyConstraints.None;
        audiosource.Stop();
        MainEngineParticle.Stop();
        audiosource.PlayOneShot(DeathExplosionAudio, 0.2f);
        DeathExplosionParticle.Play();
        // invoke runs after a timer
        Invoke("LoadLossLevel", DyingSeconds);
    }

    private void CollideLandingpad() {
        // process win screen or next level 
        print("Landing pad reached");
        gameState = GameState.Transcending;
        rigidBody.constraints = RigidbodyConstraints.None;
        audiosource.Stop();
        MainEngineParticle.Stop();
        audiosource.PlayOneShot(SuccessAudio, 0.7f);
        SuccessParticle.Play();
        // invoke runs after a timer
        Invoke("LoadNextLevel", DyingSeconds);
    }

    
    private void LoadLossLevel() {
        SceneManager.LoadScene("Lose");
    }

    
    private void LoadNextLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public bool IsStuckLanding() {
        // return true if both legs collided with landingpad
        return booster_L_landed && booster_R_landed;
    }

}
