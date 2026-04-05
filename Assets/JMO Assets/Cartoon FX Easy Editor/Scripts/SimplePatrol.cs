using UnityEngine;

public class SimplePatrol : MonoBehaviour
{
    public float speed = 3.0f;
    // Speed of the patrolm.
    public float distance = 5.0f;
    // Distance of the patrol.
    private Vector3 startPos;
    private bool movingForward = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        float distanceFromStart = Vector3.Distance(startPos, transform.position);
        if (distanceFromStart >= distance)
        {
            turnaround();
        }
    }
    void turnaround()
    {
        movingForward = !movingForward;
        transform.Rotate(0, 180, 0);
        startPos = transform.position;
    }
}
