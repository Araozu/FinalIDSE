using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorController : MonoBehaviour
{
    private Rigidbody2D _rb;

    private float driftFactor = 0.1f;
    private float acelerationFactor = 1000f;
    public float turnFactor = 1.0f;

    private float acelerationInput = 0;
    private float steeringInput = 0;

    private float rotationAngle = 0;


    private void Start()
    {
        Application.targetFrameRate = 60;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var vector2 = Vector2.zero;
        vector2.x = Input.GetAxis("Horizontal");
        vector2.y = Input.GetAxis("Vertical");

        SetInputVector(vector2);

        // Velocidad del vehiculo (dirigida hacia adelante)
        var forwardVelocity = (transform.up * _rb.velocity).magnitude;
    }

    private void ApplyEngineForce()
    {
        // Friccion
        if (acelerationInput == 0)
        {
            _rb.drag = Mathf.Lerp(_rb.drag, 2.0f, Time.fixedDeltaTime * 2);
        }
        else
        {
            _rb.drag = 0;
        }

        // Hacer que el frenado sea mas fuerte
        if (acelerationInput < 0)
        {
            acelerationInput *= 3f;
        }

        // Fuerza del motor
        var fuerza = transform.up * acelerationInput * acelerationFactor * Time.deltaTime;

        _rb.AddForce(fuerza, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        // Evitar rotacion si el vehiculo no avanza
        var minSpeed = _rb.velocity.magnitude / 8;
        minSpeed = Mathf.Clamp01(minSpeed);

        // Hacer que el vehiculo gire menos al inicio, pero luego gire mas.
        if (steeringInput == 0 && turnFactor >= 1.0f)
        {
            turnFactor -= 0.01f;
        } else if (steeringInput != 0 && turnFactor <= 1.5f)
        {
            turnFactor += 0.01f;
        }

        rotationAngle -= steeringInput * turnFactor * minSpeed;

        _rb.MoveRotation(rotationAngle);
    }

    private void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        acelerationInput = inputVector.y;
    }

    private void RemoveOrthogonalForces()
    {
        var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

        _rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }

    private void FixedUpdate()
    {
        ApplyEngineForce();
        RemoveOrthogonalForces();
        ApplySteering();
    }
}