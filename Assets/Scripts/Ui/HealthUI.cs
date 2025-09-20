using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{

    [SerializeField] private Image _heartPrefab;
    [SerializeField] private Transform _heartsParent;

    private List<Image> _hearts = new List<Image>();

    private void Start()
    {
        CreateHearts(3);
        Health.OnHealthChanged += UpdateHearts;
    }

    private void OnDestroy()
    {
        Health.OnHealthChanged -= UpdateHearts;
    }

    private void CreateHearts(int maxHealth)
    {
        for (int i = 0; i < maxHealth; i++)
        {
            var heart = Instantiate(_heartPrefab, _heartsParent);
            _hearts.Add(heart);
        }
    }

    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < _hearts.Count; i++)
        {
            _hearts[i].enabled = i < currentHealth;
        }
    }
}
