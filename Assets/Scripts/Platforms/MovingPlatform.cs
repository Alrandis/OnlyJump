using UnityEngine;

public class MovingPlatform : PlatformBase
{
    [SerializeField] private float _speed = 2f;
    private float _speedOfset = 0f;
    [SerializeField] private float _distance = 3f;

    private Vector3 _startPos;
    private int _direction = 1;

    private float newX = 0f;
    private float minX = 0f;
    private float maxX = 0f;

    private Vector3 _lastPosition;

    public Vector2 CurrentVelocity { get; private set; }

    public override void ResetPlatform()
    {
        _startPos = transform.position;
        _lastPosition = transform.position;

        _speedOfset = Random.Range(0f, 0.5f);
        _direction = Random.value < 0.5f ? 1 : -1;
    }

    private void Update()
    {
        float speed = (_speed + _speedOfset) * _direction;
        newX = transform.position.x + speed * Time.deltaTime;

        minX = _startPos.x - _distance;
        maxX = _startPos.x + _distance;

        if (newX > maxX)
        {
            newX = maxX;
            _direction = -1;
        }
        else if (newX < minX)
        {
            newX = minX;
            _direction = 1;
        }

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Вычисляем реальную скорость
        CurrentVelocity = (transform.position - _lastPosition) / Time.deltaTime;

        _lastPosition = transform.position;
    }
}
