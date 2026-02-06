using UnityEngine;

public class FlyingMovement : EnemyMovement
{
    public float Speed = 2f;
    public float Range = 2.5f; // радиус по оси X

    private float _leftLimit;
    private float _rightLimit;
    private bool _movingRight = true;

    private Vector3 _initialScale;

    private void Start()
    {
        // запоминаем изначальный масштаб (важно!)
        _initialScale = transform.localScale;

        // задаЄм границы вокруг позиции спавна
        _leftLimit = transform.position.x - Range;
        _rightLimit = transform.position.x + Range;

        UpdateFlip();
    }

    public override void Tick()
    {
        // двигаем врага по оси X
        float moveStep = Speed * Time.deltaTime * (_movingRight ? 1 : -1);
        transform.Translate(Vector2.right * moveStep);

        // если достигли границы Ч мен€ем направление
        if (_movingRight && transform.position.x >= _rightLimit)
        {
            _movingRight = false;
            UpdateFlip();
        }
        else if (!_movingRight && transform.position.x <= _leftLimit)
        {
            _movingRight = true;
            UpdateFlip();
        }
    }

    private void UpdateFlip()
    {
        // если летим вправо Ч нормальный масштаб
        // если влево Ч зеркалим по X
        transform.localScale = new Vector3(
            _movingRight ? _initialScale.x : -_initialScale.x,
            _initialScale.y,
            _initialScale.z
        );
    }
}

