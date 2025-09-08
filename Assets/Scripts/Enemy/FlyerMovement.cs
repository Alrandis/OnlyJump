using UnityEngine;

public class FlyingMovement : EnemyMovement
{
    public float Speed = 2f;
    public float Range = 2.5f; // ������ �� ��� X

    private float _leftLimit;
    private float _rightLimit;
    private bool _movingRight = true;

    private void Start()
    {
        // ����� ������� ������ ������� ������
        _leftLimit = transform.position.x - Range;
        _rightLimit = transform.position.x + Range;
    }

    public override void Tick()
    {
        // ������� ����� �� ��� X
        float moveStep = Speed * Time.deltaTime * (_movingRight ? 1 : -1);
        transform.Translate(Vector2.right * moveStep);

        // ���� �������� ������� � ������ �����������
        if (_movingRight && transform.position.x >= _rightLimit)
        {
            _movingRight = false;
        }
        else if (!_movingRight && transform.position.x <= _leftLimit)
        {
            _movingRight = true;
        }
    }
}
