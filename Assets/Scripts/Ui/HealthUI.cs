using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{

    [SerializeField] private Image heartPrefab;
    [SerializeField] private Transform heartsParent;

    private List<Image> hearts = new List<Image>();

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
            var heart = Instantiate(heartPrefab, heartsParent);
            hearts.Add(heart);
        }
    }

    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
}
