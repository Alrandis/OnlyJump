using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{

    [SerializeField] private PlayerDataSO _playerData; // Назначь в инспекторе

    [Header("Параметры здоровья")]
    [SerializeField] private int _maxHealth = 3;
    //public int CurrentHealth { get; private set; }
    public int CurrentHealth = 0;

    [Header("Параметры подбрасывания")]
    [SerializeField] private float _knockbackForce = 5f; // сила отброса

    public bool IsInvulnerable = false;

    public static Action OnPlayerDown;          // показ экрана смерти
    public static Action OnPlayerDeadConfirmed; // финальная смерть

    public static Action<int> OnHealthChanged;

    private bool _deathPending = false;

    private void Awake()
    {
        CurrentHealth = _maxHealth;
    }

    // Применяем урон и knockback
    public void TakeDamage(int damage)
    {
        if (IsInvulnerable || _deathPending) return;

        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);

        StartCoroutine(BeInvulnerable());

        if (CurrentHealth <= 0)
        {
            StartCoroutine(EnterDeathState());
        }
    }

    private IEnumerator EnterDeathState()
    {
        yield return new WaitForSeconds(0.3f);
        _deathPending = true;
        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        OnPlayerDown?.Invoke();

        if (SceneManager.GetActiveScene().name != "EndlessAdvances" && SceneManager.GetActiveScene().name != "EndlessNormal")
            ConfirmDeath();
    }

    public void ConfirmDeath()
    {
        if (!_deathPending) return;
        _deathPending = false;
        Death();
    }

    private IEnumerator BeInvulnerable()
    {
        IsInvulnerable = true;
        yield return new WaitForSeconds(1);
        IsInvulnerable = false;
    }

    public void RestoreHealth()
    {
        _deathPending = false;
      

        CurrentHealth = _maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);

        // Сбрасываем состояние в SO, чтобы скрипты движения не "глючили"
        if (_playerData != null)
        {
            _playerData.IsKnockedBack = false;
            _playerData.IsJumping = false;
            _playerData.CurrentVelocity = Vector2.zero;
        }

        gameObject.SetActive(true);

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = true;
        }

        StartCoroutine(BeInvulnerable());
    }

    private void Death()
    {
        
        OnPlayerDeadConfirmed?.Invoke();

        YG2.saves.DeathCount++;
         if (SceneManager.GetActiveScene().name != "EndlessAdvances" && SceneManager.GetActiveScene().name != "EndlessNormal")
            YG2.saves.Levels[SceneManager.GetActiveScene().buildIndex - 1].TryCount++;

        YG2.SaveProgress();
        AchiveManager.Instance.DeathCheck();
        gameObject.SetActive(false);
    }

    public void Heal()
    {
        if (CurrentHealth == 3) return;

        CurrentHealth++;
        OnHealthChanged?.Invoke(CurrentHealth);
    }
}
