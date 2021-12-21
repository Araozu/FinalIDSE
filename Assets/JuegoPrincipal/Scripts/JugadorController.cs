using Unity.Mathematics;
using UnityEngine;

public class JugadorController : MonoBehaviour
{
    private Rigidbody2D _rb;

    private const float DriftFactor = 0.1f;
    private const float AcelerationFactor = 750f;
    public float turnFactor = 0.5f;

    private float _acelerationInput = 0;
    private float _steeringInput = 0;

    private float _rotationAngle = 0;

    private bool _movimientoActivado = true;

    private void Start()
    {
        Application.targetFrameRate = 60;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!_movimientoActivado) return;

        var vector2 = Vector2.zero;
        vector2.x = Input.GetAxis("Horizontal");
        vector2.y = Input.GetAxis("Vertical");

        SetInputVector(vector2);
    }

    private void ApplyEngineForce()
    {
        // Si la velocidad del auto es negativa, detenerlo
        // TODO: Modo retroceso
        var forwardVelocity = transform.up * _rb.velocity;
        var esMovimientoHaciaAdelante = forwardVelocity.x > 0 || forwardVelocity.y > 0;
        if (_acelerationInput <= 0 && !esMovimientoHaciaAdelante)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        // Friccion
        _rb.drag = _acelerationInput == 0 ? 0.2f : 0;

        // Hacer que el frenado sea mas fuerte
        if (_acelerationInput < 0)
        {
            _acelerationInput *= 4f;
        }

        // Fuerza del motor
        var fuerza = transform.up * _acelerationInput * AcelerationFactor * Time.deltaTime;

        _rb.AddForce(fuerza, ForceMode2D.Force);
    }

    private void ApplySteering()
    {
        // Evitar rotacion si el vehiculo no avanza
        var minSpeed = _rb.velocity.magnitude / 8;
        minSpeed = Mathf.Clamp01(minSpeed);

        // Hacer que el vehiculo gire menos al inicio, pero luego gire mas.
        if (_steeringInput == 0 && turnFactor >= 0.5f)
        {
            turnFactor -= 0.5f;
        }
        else if (_steeringInput != 0 && turnFactor <= 2.5f)
        {
            turnFactor += 0.05f;
        }

        // Debug.Log("Diferencia: " + (steeringInput * turnFactor * minSpeed));
        _rotationAngle -= _steeringInput * turnFactor * minSpeed;

        _rb.MoveRotation(_rotationAngle);
    }

    private void SetInputVector(Vector2 inputVector)
    {
        _steeringInput = inputVector.x;
        _acelerationInput = inputVector.y;
    }

    private void RemoveOrthogonalForces()
    {
        var forwardVelocity = transform.up * Vector2.Dot(_rb.velocity, transform.up);
        var rightVelocity = transform.right * Vector2.Dot(_rb.velocity, transform.right);

        _rb.velocity = forwardVelocity + rightVelocity * DriftFactor;
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
    public float Velocidad()
    {
        var velocidadAdelante = transform.up * _rb.velocity;
        var esMovimientoHaciaAdelante = velocidadAdelante.x > 0 || velocidadAdelante.y > 0;
        var magnitudVelocidad = math.floor(velocidadAdelante.magnitude * 4f);

        return esMovimientoHaciaAdelante ? magnitudVelocidad : -magnitudVelocidad;
    }

    public void DesactivarMovimiento()
    {
        _movimientoActivado = false;
    }

    public void ActivarMovimiento()
    {
        _movimientoActivado = true;
    }
}