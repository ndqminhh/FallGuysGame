using UnityEngine;

public class KnockbackHazard : MonoBehaviour
{
    public float hitStrength = 12f;
    public float hitCooldown = 0.8f;

    private float nextHitTime;

    void OnTriggerEnter(Collider other)
    {
        TryHit(other);
    }    

    void OnTriggerStay(Collider other)
    {
        TryHit(other); 
    }

    void TryHit(Collider other)
    {
        if (Time.time < nextHitTime)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player == null)
            return;

        Vector3 away = other.transform.position - transform.position;

        away.y = 0f;

        if (away.sqrMagnitude < 0.01f)
            away = transform.forward;

        player.ApplyKnockback(away.normalized, hitStrength);

        nextHitTime = Time.time + hitCooldown;
    }    
}
