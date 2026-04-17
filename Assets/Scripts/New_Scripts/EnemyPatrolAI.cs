using UnityEngine;
using UnityEngine.UI;

public class EnemyPatrolAI : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty difficulty = Difficulty.Medium;

    [Header("Health & UI")]
    public Slider healthBar;
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Movement")]
    public float moveSpeed = 3f;
    public float wallDetectionRange = 2f;
    public LayerMask obstacleLayer;

    [Header("Attack")]
    public Transform player;
    public float detectionRange = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float stoppingDistance = 5f;
    public float bulletSpeed = 50f; // Added this variable to fix the error

    private float fireRate = 1f;
    private float baseAccuracy = 1f;
    private float nextFireTime;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        UpdateUI();
    }

    void Update()
    {
        if (player == null || currentHealth <= 0) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= detectionRange) AttackandChase();
        else ContinuousPatrol();

        // Animation logic
        if (anim != null)
        {
            anim.SetBool("isWalking", dist > stoppingDistance || dist > detectionRange);
            // If they are in detection range, they should probably be in an "Aim" state
            anim.SetBool("isAiming", dist <= detectionRange);
        }
    }

    void ContinuousPatrol()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, wallDetectionRange, obstacleLayer))
        {
            transform.Rotate(0, Random.Range(100, 200), 0);
        }
    }

    void AttackandChase()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);

        if (dist > stoppingDistance)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        transform.LookAt(targetPos);

        if (Time.time >= nextFireTime)
        {
            shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void shoot()
    {
        if (firePoint == null || bulletPrefab == null || player == null) return;

        // Calculate direction toward player chest (Vector3.up * 1.5f roughly)
        Vector3 targetDirection = (player.position + Vector3.up * 1.5f) - firePoint.position;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);

        float accuracy = CalculateCurrentAccuracy();
        float currentSpread = (1f - accuracy) * 10f;
        Quaternion spreadRotation = Quaternion.Euler(
            Random.Range(-currentSpread, currentSpread),
            Random.Range(-currentSpread, currentSpread),
            0f
        );

        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, lookRotation * spreadRotation);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Setup("Player", 10f);
        }

        Rigidbody bulletRb = bulletObj.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = bulletObj.transform.forward * bulletSpeed;
        }
    }

    // Added this method to fix the missing method error
    float CalculateCurrentAccuracy()
    {
        float healthPercent = currentHealth / maxHealth;
        if (healthPercent < 0.25f) return baseAccuracy * 0.3f;
        if (healthPercent < 0.5f) return baseAccuracy * 0.6f;
        return baseAccuracy;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateUI();
        if (currentHealth <= 0) Destroy(gameObject);
    }

    void UpdateUI()
    {
        if (healthBar != null) healthBar.value = currentHealth / maxHealth;
    }
}