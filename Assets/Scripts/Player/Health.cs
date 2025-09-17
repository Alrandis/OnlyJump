using System;
using System.Collections;
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

    private bool _isInvulnerable = false;

    private Rigidbody2D _rb;

    public static Action OnPlayerDead;
    public static Action<int> OnHealthChanged;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    // Применяем урон и knockback
    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return;

        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);

        StartCoroutine(BeInvulnerable());
        // Проверка смерти
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator BeInvulnerable()
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(1);
        _isInvulnerable = false;
    }

    private void Die()
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.3f);
        OnPlayerDead?.Invoke();

        gameObject.SetActive(false);
    }
}
