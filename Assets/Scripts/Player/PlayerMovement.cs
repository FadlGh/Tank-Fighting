using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Car Settings")]
    public float driftFactor;
    public float accelerationFactor;
    public float rotationFactor;
    public float maxSpeed;

    private float accelerationInput;
    private float steeringInput;
    private float rotationAngle;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        SetInputVector(inputVector);
    }

    void FixedUpdate()
    {
        ApplyEngineForce();

        KillOrthogonalVelocity();

        ApplySteering();
    }

    void ApplyEngineForce()
    {
        //Calculate how much "forward" we are going in terms of direction of our velocity
        float velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        //Limit so we cannot go faster than the max speed in the "forward" direction
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        //Limit so we cannot go faster than the max speed in the "direction" direction
        if (velocityVsUp < -maxSpeed * 0.5f & accelerationInput < 0)
            return;

        //Limit so we cannot go faster in any direction while acceleratings
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        //Apply drag if there is no accelerationInput so the car stops when the player les go of the button
        if (accelerationInput == 0)
            rb.drag = Mathf.Lerp(rb.drag, 5.0f, Time.fixedDeltaTime * 3f);
        else rb.drag = 0;
        
        //Create force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;
        
        //Apply force and pushes the car forward
        rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);

        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    void ApplySteering()
    {
        //Limit the cars ability to turn when moving slowly
        float minSpeedBeforeAllowTurningFactor = (rb.velocity.magnitude / 8);
        minSpeedBeforeAllowTurningFactor = Mathf.Clamp01(minSpeedBeforeAllowTurningFactor);

        if(accelerationInput < 0)
            rotationAngle -= -steeringInput * rotationFactor * minSpeedBeforeAllowTurningFactor;
        else
            //Update the rotation angle based on input
            rotationAngle -= steeringInput * rotationFactor * minSpeedBeforeAllowTurningFactor;

        //Apply steering by rotating the car object
        rb.MoveRotation(rotationAngle);
    }

    void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
