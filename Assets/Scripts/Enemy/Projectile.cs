using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage = 1;
    public float LifeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, LifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            PlayerAirControl playerAirControl = collision.GetComponent<PlayerAirControl>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(Damage);

                Vector2 knockback = new Vector2(
                   collision.transform.position.x < transform.position.x ? -5f : 5f, 3f);
                playerAirControl.Knockback(knockback);
            }

            Destroy(gameObject);
        }
    }
}
