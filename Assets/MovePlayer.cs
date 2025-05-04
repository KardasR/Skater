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

    #endregion Public Fields

    #region Private Methods

    /// <summary>
    /// This will move the player depending on what controls the user is pressing
    /// </summary>
    private void Move()
    {
        if (_inputSprinting)
        {
            _inputAD    *= SprintMod;
            _inputWS    *= SprintMod;
        }

        Vector3 move = _controller.transform.TransformDirection(new Vector3(_inputAD, 0, _inputWS));

        // x - left / right
        // y - up / down
        // z - forward / backward
        _controller.Move(move);
    }

    /// <summary>
    /// Move the camera around
    /// </summary>
    private void Look()
    {
        // rotation nation
        Vector3 curRotation = _controller.transform.eulerAngles;
        curRotation.y += _inputLookX;

        _controller.transform.rotation = Quaternion.Euler(curRotation);
    }

    #endregion Private Methods

    #region Core Unity Methods

    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {
        _controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        _inputAD            = Input.GetAxis("Horizontal") * Velocity;   // a/d
        _inputWS            = Input.GetAxis("Vertical")   * Velocity;   // w/s
        _inputSprinting     = Input.GetAxis("Fire3") == 1f;
    }

    /// <summary>
    /// Happens after update is called
    /// </summary>
    void LateUpdate()
    {
        // right pos / left neg
        // up pos / down neg
        _inputLookX = Input.GetAxis("Mouse X") * Sensitivity;
        Look();
    }

    // do all of the movement here as this is nice for rigid body movement
    void FixedUpdate()
    {
        Move();
    }

    #endregion Core Unity Methods
}
