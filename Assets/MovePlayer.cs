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
    /// <summary>
    /// User can only hold one puck at a time
    /// </summary>
    private Puck _heldPuck;
    
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
    /// <summary>
    /// How much to decelerate
    /// </summary>
    public float IceFriction;
    /// <summary>
    /// Top speed the user can skate
    /// </summary>
    public float MaxSpeed;
    /// <summary>
    /// How fast the puck shoots off the stick
    /// </summary>
    public float PassingSpeed;
    /// <summary>
    /// Where the puck will "stick" to
    /// </summary>
    public Transform PuckHoldPoint;

    #endregion Public Fields

    #region Private Methods

    /// <summary>
    /// Move the player as if they were skating
    /// </summary>
    private void IceMove()
    {
        Vector3 moveInput = new Vector3(_inputAD, 0, _inputWS).normalized;

        // only apply friction when velocity is non-zero
        if (_body.linearVelocity.sqrMagnitude > 0.01f)
        {
            Vector3 friction = -_body.linearVelocity.normalized * IceFriction;
            _body.AddForce(friction, ForceMode.Acceleration);
        }

        Vector3 moveDir = transform.TransformDirection(moveInput);
        Vector3 forward = transform.forward;

        // Angle between input direction and object forward
        float alignment = Vector3.Dot(moveDir.normalized, forward);

        // Allow full force if mostly forward, reduce otherwise
        float directionPenalty = Mathf.Clamp01((alignment + 1f) / 2f);

        // Optionally exaggerate penalty for sharp sideways / backwards input
        directionPenalty = Mathf.Pow(directionPenalty, 2f);

        // Apply sprint mod if sprinting
        float speed = Velocity * (_inputSprinting ? SprintMod : 1f) * directionPenalty;

        float currentSpeedInMoveDir = Vector3.Dot(_body.linearVelocity, moveDir.normalized);
        if (currentSpeedInMoveDir < MaxSpeed)
            _body.AddForce(moveDir * speed, ForceMode.Acceleration);
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

    #region Public Methods

    /// <summary>
    /// Pick up a puck. Should be called by the player sticks' trigger object
    /// </summary>
    /// <param name="puck">Puck to puck up</param>
    public void TryPickupPuck(Puck puck)
    {
        if (_heldPuck == null && puck.Pickupable)
        {
            _heldPuck = puck;
            puck.Hold(PuckHoldPoint);
        }
    }

    #endregion

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

        if (_heldPuck != null && Input.GetKeyDown(KeyCode.Space))
        {
            _heldPuck.Release(transform.up * PassingSpeed);
            _heldPuck = null;
        }
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
