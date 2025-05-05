using UnityEngine;

public class Puck : MonoBehaviour
{
    #region Private Members

    private Rigidbody _body;

    private float _pickupCooldown = 0f;

    #endregion Private Members

    #region Public Fields

    public bool IsHeld { get; private set; }

    public float PickupCooldownTime { get; set; }

    public bool Pickupable
    {
        get
        {
            return !IsHeld & _pickupCooldown <= 0f;
        }
    }

    #endregion Public Fields

    #region Public Methods

    public void Hold(Transform holdPoint)
    {
        IsHeld = true;

        transform.SetParent(holdPoint);
    }

    public void Release(Vector3 force)
    {
        IsHeld = false;
        _pickupCooldown = PickupCooldownTime;
        
        transform.SetParent(null);
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
        if (_pickupCooldown > 0f)
            _pickupCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// We need to do this here as the skater does its movement logic in fixedupdate
    /// </summary>
    void FixedUpdate()
    {
        if (IsHeld && transform.parent != null)
        {
            transform.localPosition = transform.parent.localPosition;
            Quaternion offset = Quaternion.Euler(90f, 0f, 0f);
            transform.localRotation = transform.parent.localRotation * offset;
        }
    }

    #endregion Core Unity Methods
}
