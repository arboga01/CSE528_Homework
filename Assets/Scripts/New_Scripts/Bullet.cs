using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage = 10f;
    private string targetTag = "";

    // This must be called by the shooter (Player or Enemy) right after spawning
    public void Setup(string tagToHit, float bulletDamage)
    {
        targetTag = tagToHit;
        damage = bulletDamage;
    }

    void OnTriggerEnter(Collider other)
    {
        // Safety check: if targetTag wasn't set, don't do anything
        if (string.IsNullOrEmpty(targetTag)) return;

        // 1. Check if the object we hit (or its parent) has the target tag
        if (other.CompareTag(targetTag) || (other.transform.parent != null && other.transform.parent.CompareTag(targetTag)))
        {
            // 2. Try to find PlayerHealth on this object or its parents
            PlayerHealth pHealth = other.GetComponentInParent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }

            // 3. Try to find EnemyPatrolAI on this object or its parents
            EnemyPatrolAI eHealth = other.GetComponentInParent<EnemyPatrolAI>();
            if (eHealth != null)
            {
                eHealth.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        // 4. If we hit a wall or floor (something that isn't a bullet or the shooter)
        // We check the layer to make sure the bullet doesn't hit the person who just fired it
        else if (!other.CompareTag("Bullet") && other.gameObject.layer != gameObject.layer)
        {
            Destroy(gameObject);
        }
    }
}