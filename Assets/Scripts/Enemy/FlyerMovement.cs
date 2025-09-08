using UnityEngine;

public class FlyingMovement : EnemyMovement
{
    public float Speed = 2f;
    public float Range = 2.5f; // радиус по оси X

    private float _leftLimit;
    private float _rightLimit;
    private bool _movingRight = true;

    private void Start()
    {
        // задаём границы вокруг позиции спавна
        _leftLimit = transform.position.x - Range;
        _rightLimit = transform.position.x + Range;
    }

    public override void Tick()
    {
        // двигаем врага по оси X
        float moveStep = Speed * Time.deltaTime * (_movingRight ? 1 : -1);
        transform.Translate(Vector2.right * moveStep);

        // если достигли границы — меняем направление
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
