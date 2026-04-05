using UnityEngine;

public class FlightControl : MonoBehaviour
{
    
    public float rotationspeed_pitch = 10;
    public float rotationspeed_yaw = 10;
    public float rotationspeed_roll = 10;

    public float speed = 10;

    void Start()
    {
    }
    void Update()
    {
        float pitch = 0;
        float yaw = 0;
        float roll = 0;

        if (Input.GetKey(KeyCode.W)) pitch -= rotationspeed_pitch * Time.deltaTime; // pitch down
        if (Input.GetKey(KeyCode.S)) pitch += rotationspeed_pitch * Time.deltaTime; // pitch up

        if (Input.GetKey(KeyCode.D)) yaw += rotationspeed_yaw * Time.deltaTime; // Yaw right
        if (Input.GetKey(KeyCode.A)) yaw -= rotationspeed_yaw * Time.deltaTime; // Yaw left 

        if (Input.GetKey(KeyCode.Q)) roll += rotationspeed_roll * Time.deltaTime; // Roll right
        if (Input.GetKey(KeyCode.E)) roll -= rotationspeed_roll * Time.deltaTime; // Roll left
       //if (Input.GetKey(KeyCode.LeftShift)) speed += 5 * Time.deltaTime; //control speed up
//if (Input.GetKey(KeyCode.LeftControl)) speed -= 5 * Time.deltaTime; //control speed down 

        transform.Rotate(pitch, yaw, roll);

        transform.Translate(0, 0, speed * Time.deltaTime * transform.localScale.magnitude);
    }
}
