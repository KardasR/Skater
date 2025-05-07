using UnityEngine;

public class Puck : MonoBehaviour
{
    #region Private Members

    private Rigidbody _body;
    private Transform _holdPoint;

    private float _pickupCooldown = 0f;

    #endregion Private Members

    #region Public Fields

    public bool IsHeld
    {
        get
        {
            return _holdPoint != null;
        }
    }

    public float PickupCooldownTime;

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
        _body.isKinematic = true;
        _holdPoint = holdPoint;
    }

    public void Release(Vector3 force)
    {
        _pickupCooldown = PickupCooldownTime;
        _holdPoint = null;

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
        if (_pickupCooldown > 0f)
            _pickupCooldown -= Time.deltaTime;
    }

    /// <summary>
    /// We need to do this here as the skater does its movement logic in fixedupdate
    /// </summary>
    void FixedUpdate()
    {
        if (IsHeld)
        {
            _body.MovePosition(_holdPoint.localPosition);
             Quaternion offset = Quaternion.Euler(90f, 0f, 0f);
            _body.MoveRotation(_holdPoint.localRotation * offset);
        }
    }

    #endregion Core Unity Methods
}
