using UnityEngine;

/// <summary>
/// Will be attatched to the skaters stick so that when it collides with the pucks sphere collider, the player "picks up" the puck.
/// </summary>
public class PuckPickupTrigger : MonoBehaviour
{
    public MovePlayer skater;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Puck") && collider.TryGetComponent(out Puck puck))
        {
            skater.TryPickupPuck(puck);
        }
    }
}
