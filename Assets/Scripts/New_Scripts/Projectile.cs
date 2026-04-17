using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyPatrolAI enemy = collision.gameObject.GetComponent<EnemyPatrolAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}