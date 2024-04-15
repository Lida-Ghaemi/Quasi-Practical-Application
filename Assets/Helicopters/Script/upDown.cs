using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class upDown : MonoBehaviour
{
    public float speed = 5f; // Adjust speed as needed
    public float rotationSpeed = 100f; // Adjust rotation speed as needed
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.useGravity = false; // Disable gravity
    }

    void Update()
    {
        Vector3 movementDirection = Vector3.zero;
        float rotationDirectionPitch = 0f;
        float rotationDirectionRoll = 0f;


        // Check for input and set movement direction accordingly
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.T))
        {
            movementDirection += transform.up;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.G))
        {
            movementDirection -= transform.up;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movementDirection -= transform.right;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movementDirection += transform.right;
        }
        if (Input.GetKey(KeyCode.G))
        {
            movementDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.T))
        {
            movementDirection -= transform.forward;
        }
        if (Input.GetKey(KeyCode.F))
        {
            rotationDirectionPitch -= 1f;
        }
        if (Input.GetKey(KeyCode.H))
        {
            rotationDirectionPitch += 1f;
        }
        if (Input.GetKey(KeyCode.J))
        {
            rotationDirectionRoll -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotationDirectionRoll += 1f;
        }

        // Normalize the movement direction if necessary
        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }

        // Apply the movement
        rb.velocity = movementDirection * speed;
        transform.Rotate(Vector3.right, rotationDirectionPitch * rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up, rotationDirectionRoll * rotationSpeed * Time.deltaTime);
    }
}
