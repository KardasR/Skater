using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    #region Private Members
    
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
    /// <summary>
    /// Physics-based body for the player
    /// </summary>
    private Rigidbody _body;
    
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

    public float IceFriction;

    public float MaxSpeed;

    #endregion Public Fields

    #region Private Methods

    /// <summary>
    /// Meant to simulate the ice
    /// </summary>
    private void IceMove()
    {
        Vector3 moveInput = new Vector3(_inputAD, 0, _inputWS).normalized;
        Vector3 moveDir = transform.TransformDirection(moveInput);

        if (moveInput.magnitude > 0)
        {
            // apply force forward in the direction off the player
            if (_body.linearVelocity.magnitude < MaxSpeed)
            {
                float mod = _inputSprinting ? SprintMod : 1f;
                _body.AddForce(mod * Velocity * moveDir, ForceMode.Acceleration);
            }
        }
        else
        {
            _body.AddForce(-_body.linearVelocity.normalized * IceFriction, ForceMode.Acceleration);
        }
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
        _body = GetComponent<Rigidbody>();
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
        _inputLookX         = Input.GetAxis("Mouse X");      // right pos / left neg
        _inputSprinting     = Input.GetAxis("Fire3") == 1f;
    }

    /// <summary>
    /// Happens after update is called
    /// </summary>
    void LateUpdate()
    {
        Look();
    }

    // do all of the movement here as this is nice for rigid body movement
    void FixedUpdate()
    {
        IceMove();
    }

    #endregion Core Unity Methods
}
