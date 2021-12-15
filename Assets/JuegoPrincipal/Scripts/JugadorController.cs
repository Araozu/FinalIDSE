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

        // Velocidad del vehiculo (en cualquier sentido)
        var forwardVelocity = (transform.up * _rb.velocity).magnitude;
    }

    private void ApplyEngineForce()
    {
        // Si la velocidad del auto es negativa, detenerlo
        // TODO: Modo retroceso
        var forwardVelocity = transform.up * _rb.velocity;
        var esMovimientoHaciaAdelante = forwardVelocity.x > 0 || forwardVelocity.y > 0;
        if (acelerationInput <= 0 && !esMovimientoHaciaAdelante)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

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
        }
        else if (steeringInput != 0 && turnFactor <= 1.5f)
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

    /**
     * Devuelve la velocidad del vehiculo en km/h, positivo si va hacia adalente,
     * negativo si va hacia atras
     */
    public float GetVelocity()
    {
        var velocidadAdelante = transform.up * _rb.velocity;
        var esMovimientoHaciaAdelante = velocidadAdelante.x > 0 || velocidadAdelante.y > 0;
        var magnitudVelocidad = velocidadAdelante.magnitude * 2.5f;

        return esMovimientoHaciaAdelante ? magnitudVelocidad : -magnitudVelocidad;
    }
}