using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public Transform resetLocation;

    private void OggerEnter(Collider other)
    {
        if (other.CompareTag("Puck"))
        {
            Debug.Log("GOAL");

            // GOAAAAAAAAAAAAAAAAAAAAAAAAAAALLLLLLLLLLLLLLLAAAAAAASSSSSSSSSSSSSSOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
            Rigidbody body = other.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.transform.SetPositionAndRotation(resetLocation.position, resetLocation.rotation);
            }
        }
    }
}
