using UnityEngine;

public class Puck : MonoBehaviour
{
    #region Private Fields

    /// <summary>
    /// Rigidbody component of the puck. Mainly used to set the iskinematic property to avoid physics issues.
    /// <para/>
    /// iskinematic property is true when the puck is held, false when it's not.
    /// </summary>
    private Rigidbody _body;

    /// <summary>
    /// How much time is left until the puck can be picked up.
    /// </summary>
    private float _pickupCooldownTimeLeft = 0f;

    #endregion Private Fields

    #region Public Fields

    /// <summary>
    /// How much time to wait until letting the puck be pickupable.
    /// </summary>
    public float PickupCooldownTime;

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Is the puck held?
    /// </summary>
    public bool IsHeld
    {
        get; private set;
    }

    /// <summary>
    /// Can the puck be picked up?
    /// </summary>
    public bool Pickupable
    {
        get
        {
            return !IsHeld && _pickupCooldownTimeLeft <= 0f;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Mark the puck is held and turn physics on.
    /// </summary>
    public void Hold()
    {
        IsHeld = true;
        _body.isKinematic = true;
    }

    /// <summary>
    /// Start the cooldown, mark the puck is no longer held, push the puck forward.
    /// </summary>
    /// <param name="force"></param>
    public void Release(Vector3 force)
    {
        _pickupCooldownTimeLeft = PickupCooldownTime;

        IsHeld = false;
        _body.isKinematic = false;
        _body.AddForce(force, ForceMode.Impulse);
    }

    #endregion Public Methods

    #region Core Unity Methods

    void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_pickupCooldownTimeLeft > 0f)
            _pickupCooldownTimeLeft = Mathf.Max(0, _pickupCooldownTimeLeft - Time.deltaTime);
    }

    #endregion Core Unity Methods
}
