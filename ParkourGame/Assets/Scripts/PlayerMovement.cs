using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;
    public float jumpForce = 2f;
    private bool isJumping = false;
    private bool isGrounded = true;
    // Up (W), Left (A), Down (S), Right (D)
    private bool[] isMoving = { false, false, false, false };
    private KeyCode[] wasd = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    private KeyCode[] arrows = { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKey(wasd[i]) || Input.GetKey(arrows[i]))
            {
                isMoving[i] = true;
            }
            else
            {
                isMoving[i] = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 movementVector = Vector3.zero;
        Vector3[] directions = { transform.forward, -transform.right, -transform.forward, transform.right };

        for (int i = 0; i < 4; i++)
        {
            if (isMoving[i])
            {
                movementVector += directions[i];
            }
        }

        movementVector = movementVector.normalized;
        rb.AddForce(movementVector * speed, ForceMode.Force);

        if (isJumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isJumping = false;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void OnCollisionStay(Collision collision)
    {
        bool grounded = false;

        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.65f)
            {
                grounded = true;
                break;
            }
        }

        isGrounded = grounded;
    }
}
