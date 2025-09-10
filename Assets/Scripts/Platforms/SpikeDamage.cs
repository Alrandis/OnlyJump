using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health hp = other.GetComponent<Health>();
            PlayerAirControl playerAirControl = other.GetComponent<PlayerAirControl>();   
            if (hp != null && playerAirControl != null)
            {
                hp.TakeDamage(damage);
                Vector2 knockback = new Vector2(
                  other.transform.position.x < transform.position.x ? -5f : 5f, 3f);
                playerAirControl.Knockback(knockback);
            }

        }
    }
}
