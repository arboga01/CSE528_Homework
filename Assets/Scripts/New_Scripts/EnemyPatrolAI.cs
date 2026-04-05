using UnityEngine;
using UnityEngine.UI;

public class EnemyPatrolAI : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    [Header("Difficulty Settings")]
    public Difficulty difficulty = Difficulty.Medium;

    [Header("Health & UI")]
    public Slider healthBar;
    private float maxhealth;
    private float currentHealth;

    [Header("Movement Settings")]
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float wallDetectionRange = 2f;
    public LayerMask obstacleLayer;
    private int currentWaypointIndex = 0;


    [Header("Detection Settings (Should be set by Difficulty")]
    public Transform player;
    public float detectionRange;
    public LayerMask playerLayer;

    [Header("Attack Settings (should be set by Difficulty)")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    private float fireRate;
    private const float CONSTANT_BULLET_SPEED = 100f;
    private float nextFireTime;
    private float baseAccuracy;

    [Header("Animation")]
    private Animator anim;

    private enum State { Patrolling, Chasing, Attacking }
    private State currentState = State.Patrolling;

    void Awake()
    {
        anim = GetComponent<Animator>();
        ApplyDifficultySettings();
        currentHealth = maxhealth;

        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            canvas.worldCamera = Camera.main;
        }
        UpdateUI();

    }

    void ApplyDifficultySettings()
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                maxhealth = 50f;
                detectionRange = 5f;
                fireRate = 1.0f;
                baseAccuracy = 0.5f;
                break;
            case Difficulty.Medium:
                maxhealth = 100f;
                detectionRange = 10f;
                fireRate = 1.0f;
                baseAccuracy = 0.7f;
                break;
            case Difficulty.Hard:
                maxhealth = 150f;
                detectionRange = 15f;
                fireRate = 1.5f;
                baseAccuracy = 0.8f;
                break;
        }
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        currentState = (distanceToPlayer <= detectionRange) ? State.Chasing : State.Patrolling;

        if (currentState == State.Patrolling)
        {
            RoamingPatrol();
        }
        else
        {
            AttackandChase();
        }
        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }

    }
    void RoamingPatrol()
    {
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * wallDetectionRange, Color.red);
        if (Physics.Raycast(ray, out hit, wallDetectionRange, obstacleLayer))
        {
            float randomTurn = Random.Range(100f, 260f);
            transform.Rotate(0, randomTurn, 0);
        }
        else
        {
            Patrol();
        }
    }
    float CalculateCurrentAccuracy()
    {
        float accuracy = baseAccuracy;
        float healthPercent = currentHealth / maxhealth;

        if (healthPercent < 0.25f)
            accuracy *= 0.3f; //70% accuracy drop at low health
        else if (healthPercent < 0.5f)
            accuracy *= 0.6f; //40% accuracy drop at medium health
        return accuracy;

    }
    void shoot()
    {
        float accuracy = CalculateCurrentAccuracy();

        float maxSpread = 20f;
        float currentSpread = (1f - accuracy) * maxSpread;

        Quaternion spreadRotation = Quaternion.Euler(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread),
            0f
        );

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * spreadRotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)

            bulletRb.linearVelocity = bullet.transform.forward * CONSTANT_BULLET_SPEED;
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateUI();
        if (currentHealth <= 0) Die();
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = currentHealth / maxhealth;
    }
    void Die()
    {
        Destroy(gameObject);
    }
    void Patrol()
    {
        if (waypoints.Length == 0) return;
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
    void AttackandChase() { 
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed* Time.deltaTime);
        Vector3 direction = (targetPos - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime* 5f);
        }

        if (Time.time >= nextFireTime)
        {
        shoot();
        nextFireTime = Time.time + 1f / fireRate;
        }
    }
}
