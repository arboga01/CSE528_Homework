using UnityEngine;

public class BodyPart : MonoBehaviour
{

    public EnemyPatrolAI mainAI;

    public float DamageMultiplier = 100f;

    public void RecieveHit(float incomingDamage)
    {
        mainAI.TakeDamage(incomingDamage * DamageMultiplier);
    }
}