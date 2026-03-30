using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public AudioClip spottedSound;

    private Transform player;
    private Vector3 playerStartPosition;
    private AudioSource audioSource;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerStartPosition = player.position;
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (player == null) return;

        if (CanSeePlayer())
            ResetPlayer();
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > viewDistance)
            return false;

        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        if (angle > viewAngle / 2f)
            return false;

        if (Physics.Raycast(transform.position, directionToPlayer.normalized,
                            distanceToPlayer, obstacleMask))
            return false;

        return true;
    }

    void ResetPlayer()
    {
        audioSource.PlayOneShot(spottedSound);

        player.position = playerStartPosition;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = Vector3.zero;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2f);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
    }

    Vector3 DirFromAngle(float angleDegrees)
    {
        float rad = (transform.eulerAngles.y + angleDegrees) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            ResetPlayer();
    }
}