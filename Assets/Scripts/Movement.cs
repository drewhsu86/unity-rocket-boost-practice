using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;
    [SerializeField] float movementPeriod;
    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // sin(2pi) = sin(0) = 0 (one period is 2pi)
        // if desired frequency is x cycle/seconds
        // sin(2pi*w*t) is a sine wave that has this frequency
        // we are just scaling our vector3 by a sin wave
        // sin wave has both plus and minus values so starting is center
        // or we could use cosine for example 
        float currTime = Time.time;
        float pi = Mathf.PI;

        if (movementPeriod <= Mathf.Epsilon) {
            return;
        }
        float freq = 1/movementPeriod;
        Vector3 differenceVector = Mathf.Sin(2*pi*freq*currTime) * movementVector;

        transform.position = startingPosition + differenceVector;
         
    }
}
