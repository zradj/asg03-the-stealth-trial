using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    public Transform startPoint;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Transform t = other.transform;

        other.attachedRigidbody.velocity = Vector3.zero;
        other.attachedRigidbody.angularVelocity = Vector3.zero;
        t.position = startPoint.position + Vector3.up;
    }
}
