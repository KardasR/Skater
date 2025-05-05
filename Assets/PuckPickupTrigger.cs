using UnityEngine;

public class PuckPickupTrigger : MonoBehaviour
{
    public MovePlayer skater;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Puck"))
        {
            Puck puck = collider.GetComponent<Puck>();
            if (puck != null)
                skater.TryPickupPuck(puck);
        }
    }
}
