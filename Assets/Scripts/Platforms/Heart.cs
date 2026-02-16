using UnityEngine;

public class Heart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out Health health))
        {
            health.Heal();
            Destroy(this.gameObject);
        }
    }
}
