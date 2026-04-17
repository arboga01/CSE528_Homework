using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // We keep this public just in case
    public float speed = 5f;
    Vector3 velocity;
    public float gravity = -9.81f;

    void Start()
    {
        // This line automatically finds the component so you don't have to drag it!
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        // Added a safety check to prevent the error if the component is missing entirely
        if (controller == null) return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}