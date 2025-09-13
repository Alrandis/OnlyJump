using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Health : MonoBehaviour
{
    [Header("��������� ��������")]
    [SerializeField] private int MaxHealth = 3;
    public int CurrentHealth { get; private set; }

    [Header("��������� �������������")]
    [SerializeField] private float KnockbackForce = 5f; // ���� �������

    private Rigidbody2D _rb;

    public static Action OnPlayerDead;
    public static Action<int> OnHealthChanged;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
    }

    // ��������� ���� � knockback
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        OnHealthChanged?.Invoke(CurrentHealth);
 
        // �������� ������
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnPlayerDead?.Invoke();

        gameObject.SetActive(false);
    }

    // �������������� �������� (�����������)
    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }
}
