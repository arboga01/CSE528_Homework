using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    private string targetTag;

    public void Setup(string tag, float dmg)
    {
        targetTag = tag;
        damage = dmg;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // 1. Try to hurt the Player
            PlayerHealth pHealth = other.GetComponent<PlayerHealth>();
            if (pHealth != null) pHealth.TakeDamage(damage);

            // 2. Try to hurt the Enemy
            EnemyPatrolAI eHealth = other.GetComponent<EnemyPatrolAI>();
            if (eHealth != null) eHealth.TakeDamage(damage);

            Destroy(gameObject);
        }
        else if (!other.CompareTag("Bullet") && other.gameObject.layer != gameObject.layer)
        {
            Destroy(gameObject);
        }
    }
}