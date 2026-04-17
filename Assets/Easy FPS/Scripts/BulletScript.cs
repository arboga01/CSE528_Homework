using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000000;
    RaycastHit hit;
    [Tooltip("Prefab of wall damange hit. The object needs 'LevelPart' tag to create decal on it.")]
    public GameObject decalHitWall;
    [Tooltip("Decal will need to be sligtly infront of the wall so it doesnt cause rendeing problems so for best feel put from 0.01-0.1.")]
    public float floatInfrontOfWall;
    [Tooltip("Blood prefab particle this bullet will create upoon hitting enemy")]
    public GameObject bloodEffect;
    [Tooltip("Put Weapon layer and Player layer to ignore bullet raycast.")]
    public LayerMask ignoreLayer;

    /*
    * Upon bullet creation, this raycast searches for corresponding tags.
    */
    void Update()
    {

        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {

            // 1. Check for the custom "Enemy" tag
            if (hit.transform.tag == "Enemy")
            {
                EnemyPatrolAI enemyScript = hit.transform.GetComponent<EnemyPatrolAI>();

                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(25f);
                }

                if (bloodEffect) Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(gameObject);
            }
            // 2. Check for the default "Dummie" tag
            else if (hit.transform.tag == "Dummie")
            {
                if (bloodEffect) Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(gameObject);
            }
            // 3. Check for the environment
            else if (hit.transform.tag == "LevelPart")
            {
                if (decalHitWall) Instantiate(decalHitWall, hit.point + hit.normal * floatInfrontOfWall, Quaternion.LookRotation(hit.normal));
                Destroy(gameObject);
            }
            // 4. If it hits anything else, still destroy the bullet
            else
            {
                Destroy(gameObject);
            }
        }

        // Failsafe: Destroy bullet after 0.1 seconds if it hits nothing
        Destroy(gameObject, 0.1f);
    }
}
