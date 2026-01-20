using UnityEngine;
using YG;

[RequireComponent(typeof(Collider2D))]
public class LavaController : MonoBehaviour
{
    [Header("Движение лавы")]
    [SerializeField] private float _baseSpeed = 0.5f;   // базовая скорость
    [SerializeField] private float _speedIncrease = 0.03f; // на сколько ускоряется с ростом игрока
    [SerializeField] private Transform _player;         // игрок для отслеживания высоты
    [SerializeField] private float _currentSpeed;

    private float _maxSpeed = 2.8f;

    private void Update()
    {
        if (_player == null) return;

        // скорость зависит от высоты игрока
        _currentSpeed = (_currentSpeed < _maxSpeed) ? _baseSpeed + _player.position.y * _speedIncrease : _maxSpeed;

        // движем лаву вверх
        transform.Translate(Vector3.up * _currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // проверяем игрока
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            // наносим 3 урона (гарантированный смертельный удар)
            health.TakeDamage(3);
            YG2.saves.DeathLava++;
            YG2.SaveProgress();
        }

        var playerAirControl = other.GetComponent<PlayerAirControl>();
        if (playerAirControl != null)
        {
            playerAirControl.Bounce(10);
        }
    }
}
