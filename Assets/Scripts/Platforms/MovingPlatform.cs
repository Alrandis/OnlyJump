using UnityEngine;

public class MovingPlatform : PlatformBase
{
    [SerializeField] private float _speed = 2f;
    private float _speedOfset = 0f;
    [SerializeField] private float _distance = 3f;

    private Vector3 _startPos;
    private int _direction = 1;

    private float speed = 0f;
    private float newX = 0f;
    private float minX = 0f;
    private float maxX = 0f;

    public override void ResetPlatform()
    {
        _startPos = transform.position;
        _speedOfset = Random.Range(0f, 0.5f);
        _direction = Random.value < 0.5f ? 1 : -1;
    }

    private void Update()
    {
        speed = (_speed + _speedOfset) * _direction;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) // игрок сверху
                {
                    // прикрепляем игрока к платформе
                    collision.collider.transform.SetParent(transform);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // убираем связь при уходе
            collision.collider.transform.SetParent(null);
        }
    }
}
