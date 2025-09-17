using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LavaController : MonoBehaviour
{
    [Header("�������� ����")]
    [SerializeField] private float _baseSpeed = 0.5f;   // ������� ��������
    [SerializeField] private float _speedIncrease = 0.05f; // �� ������� ���������� � ������ ������
    [SerializeField] private Transform _player;         // ����� ��� ������������ ������
    [SerializeField] private float _currentSpeed;

    private float _maxSpeed = 3f;

    private void Update()
    {
        if (_player == null) return;

        // �������� ������� �� ������ ������
        _currentSpeed = (_currentSpeed < _maxSpeed) ? _baseSpeed + _player.position.y * _speedIncrease : _maxSpeed;

        // ������ ���� �����
        transform.Translate(Vector3.up * _currentSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��������� ������
        var health = other.GetComponent<Health>();
        if (health != null)
        {
            // ������� 3 ����� (��������������� ����������� ����)
            health.TakeDamage(3);
        }

        var playerAirControl = other.GetComponent<PlayerAirControl>();
        if (playerAirControl != null)
        {
            // ������� 3 ����� (��������������� ����������� ����)
            playerAirControl.Bounce(10);
        }
    }
}
