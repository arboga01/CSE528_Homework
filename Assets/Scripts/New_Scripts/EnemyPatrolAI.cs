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

        if (anim != null) anim.SetBool("isWalking", true);
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
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet script = b.GetComponent<Bullet>();
        if (script != null) script.Setup("Player", 10f);

        Rigidbody rb = b.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = firePoint.forward * 50f;
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