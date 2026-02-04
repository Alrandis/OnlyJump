using System.Collections;
using UnityEngine;
using YG;

[RequireComponent(typeof(Collider2D))]
public class LavaController : MonoBehaviour
{
    [Header("ƒвижение лавы")]
    [SerializeField] private float _baseSpeed = 0.5f;
    [SerializeField] private float _speedIncrease = 0.03f;
    [SerializeField] private Transform _player;

    [Header("ќграничени€")]
    [SerializeField] private float _maxSpeed = 2.8f;

    [Header("ѕараметры воскрешени€")]
    [SerializeField] private float _reviveOffsetY = 6f;     // насколько опустить лаву
    [SerializeField] private float _slowMultiplier = 0.5f; // замедление в 2 раза
    [SerializeField] private float _slowDuration = 3f;     // сколько секунд действует

    private float _currentSpeed;
    private bool _isSlowed = false;

    private void Update()
    {
        if (_player == null) return;

        float targetSpeed = _baseSpeed + _player.position.y * _speedIncrease;
        targetSpeed = Mathf.Min(targetSpeed, _maxSpeed);

        if (_isSlowed)
            targetSpeed *= _slowMultiplier;

        _currentSpeed = targetSpeed;

        transform.Translate(Vector3.up * _currentSpeed * Time.deltaTime);
    }

    /// <summary>
    /// ¬ызываетс€ при воскрешении игрока
    /// </summary>
    public void OnPlayerRevived(float playerY)
    {
        StopAllCoroutines();

        // сдвигаем лаву ниже игрока
        float newY = Mathf.Min(transform.position.y, playerY - _reviveOffsetY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        StartCoroutine(SlowLavaTemporarily());
    }

    private IEnumerator SlowLavaTemporarily()
    {
        _isSlowed = true;
        yield return new WaitForSeconds(_slowDuration);
        _isSlowed = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(3);
            YG2.saves.DeathLava++;
            YG2.SaveProgress();
            AchiveManager.Instance.DeathLavaCheck();
        }

        var playerAirControl = other.GetComponent<PlayerAirControl>();
        if (playerAirControl != null)
        {
            playerAirControl.Bounce(10);
        }
    }
}
