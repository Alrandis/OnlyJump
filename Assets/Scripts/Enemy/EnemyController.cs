using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    public int Damage = 1;
    public float BounceForce = 10f;

    private Health _playerHealth;
    private PlayerAirControl _playerAirControl;
    [SerializeField] private Transform enemyTop;

    private void Awake()
    {
        // Ищем игрока (по тегу)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            _playerHealth = playerObj.GetComponent<Health>();
            _playerAirControl = playerObj.GetComponent<PlayerAirControl>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // Проверка: сверху ли игрок
        if (collision.transform.position.y > enemyTop.position.y)
        {
            // Враг умирает
            Die();

            // Игрок отскакивает вверх
            _playerAirControl.Bounce(BounceForce);
        }
        else
        {
            // Игрок получает урон
            _playerHealth.TakeDamage(Damage);

            //Vector2 knockbackDir = ((Vector2)collision.transform.position - (Vector2)enemyTop.position).normalized;

            Vector2 knockback = new Vector2(
               collision.transform.position.x < enemyTop.position.x ? -5f : 5f, 3f); 


            _playerAirControl.Knockback(knockback);
        }
    }

    private void Die()
    {
        Destroy(gameObject); // пока просто уничтожаем
    }
}
