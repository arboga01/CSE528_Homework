using UnityEngine;

public class Jetpatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 20.0f; // Speed of the jet
    public float turnSpeed = 2.0f;
    public float patrolDistance = 500.0f;
    public float arrivalThreshold = 5.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 currentTarget;
    private bool isTurning = false;

    void Start()
    {
        startPosition = transform.position;
        endPosition = startPosition + (transform.forward * patrolDistance);
        currentTarget = endPosition;
    }

    // Update is called once per frame
    void Update()
    {
        MoveJet();
        CheckDistance();
    }
    void MoveJet()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 directionToTarget = (currentTarget - transform.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
    void CheckDistance()
    {
        float distanceToTarget = Vector3.Distance(transform.position, currentTarget);
        if (distanceToTarget < arrivalThreshold)
        {
            if (currentTarget == endPosition)
            {
                currentTarget = startPosition;
            }
            else
            {
                currentTarget = endPosition;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPosition, endPosition);
            Gizmos.DrawSphere(currentTarget, 1.0f);
        }
    }
}