using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    [Header("Параметры здоровья")]
    [SerializeField] private int MaxHealth = 3;
    public int CurrentHealth { get; private set; }

    [Header("Параметры подбрасывания")]
    [SerializeField] private float KnockbackForce = 5f; // сила отброса

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    // Применяем урон и knockback
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        // Проверка смерти
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Здесь можно добавить анимацию смерти, перезагрузку сцены и т.д.
        Debug.Log("Игрок умер");
        // например:
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameObject.SetActive(false);
    }

    // Восстановление здоровья (опционально)
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }
}
