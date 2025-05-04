using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    #region Private Members

    /// <summary>
    /// Character controller of the player
    /// </summary>
    private CharacterController _controller;
    /// <summary>
    /// Reads input for a normalized value. negative for a, positive for d. (Double check this)
    /// </summary>
    private float _inputAD;
    /// <summary>
    /// Reads input for a normalized value. negative for s, positive for w. (Double check this)
    /// </summary>
    private float _inputWS; 
    /// <summary>
    /// Reads input to see if the user is pressing the shift key or anything else mapped to Fire3
    /// </summary>
    private bool _inputSprinting;
    /// <summary>
    /// Reads input to see if the user is moving the mouse side to side
    /// </summary>
    private float _inputLookX;
    /// <summary>
    /// Hold the eulerangles of the player without having to keep reading it from the object. Helps avoid camera issues
    /// </summary>
    private float _yaw;

    private Vector3 _currentVelocity;
    
    #endregion Private Members

    #region Public Fields

    /// <summary>
    /// Mouse sensitivity
    /// </summary>
    public float Sensitivity;
    /// <summary>
    /// Extra velocity when sprinting
    /// </summary>
    public float SprintMod;
    /// <summary>
    /// Speed the player moves
    /// </summary>
    public float Velocity;
    
    public float Acceleration;

    public float Deceleration;

    #endregion Public Fields

    #region Private Methods

    /// <summary>
    /// This will move the player depending on what controls the user is pressing
    /// </summary>
    private void Move()
    {
        float speed = _inputSprinting ? Velocity * SprintMod : Velocity;

        // movement direction in local space
        Vector3 inputDir = new Vector3(_inputAD, 0, _inputWS).normalized;

        Vector3 forward = Quaternion.Euler(0, _yaw, 0) * inputDir;

        _controller.Move(speed * Time.fixedDeltaTime * forward);
    }

    /// <summary>
    /// Meant to simulate the ice
    /// </summary>
    private void IceMove()
    {
        Vector3 inputDir = new Vector3(_inputAD, 0, _inputWS).normalized;
        Vector3 targetDirection = Quaternion.Euler(0, _yaw, 0) * inputDir;

        float targetSpeed = _inputSprinting ? Velocity * SprintMod : Velocity;
        Vector3 desiredVelocity = targetDirection * targetSpeed;

        // Accelerate toward the desired velocity
        if (desiredVelocity.magnitude > 0.1f)
        {
            _currentVelocity = Vector3.Lerp(_currentVelocity, desiredVelocity, Acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate slowly
            _currentVelocity = Vector3.Lerp(_currentVelocity, Vector3.zero, Deceleration * Time.fixedDeltaTime);
        }

        _controller.Move(_currentVelocity * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Move the camera around
    /// </summary>
    private void Look()
    {
        // rotation nation
        if (Mathf.Abs(_inputLookX) > 0.1f)
            _yaw += _inputLookX * Sensitivity;

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
    }

    #endregion Private Methods

    #region Core Unity Methods

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        _inputAD            = Input.GetAxis("Horizontal");   // a/d
        _inputWS            = Input.GetAxis("Vertical");     // w/s
        _inputSprinting     = Input.GetAxis("Fire3") == 1f;
    }

    /// <summary>
    /// Happens after update is called
    /// </summary>
    void LateUpdate()
    {
        // right pos / left neg - x
        // up pos / down neg - y
        _inputLookX = Input.GetAxis("Mouse X");
        Look();
    }

    // do all of the movement here as this is nice for rigid body movement
    void FixedUpdate()
    {
        //Move();
        IceMove();
    }

    #endregion Core Unity Methods
}
