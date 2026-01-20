using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    [Header("Параметры здоровья")]
    [SerializeField] private int _maxHealth = 3;
    public int CurrentHealth { get; private set; }

    [Header("Параметры подбрасывания")]
    [SerializeField] private float _knockbackForce = 5f; // сила отброса

    private bool _isInvulnerable = false;

    public static Action OnPlayerDead;
    public static Action<int> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = _maxHealth;
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
        YG2.saves.DeathCount++;
        if(SceneManager.GetActiveScene().name != "EternalLevel")
            YG2.saves.Levels[SceneManager.GetActiveScene().buildIndex].TryCount++;

        YG2.SaveProgress();

        gameObject.SetActive(false);
    }
}
