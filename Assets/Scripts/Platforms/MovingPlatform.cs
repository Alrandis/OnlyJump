using UnityEngine;

public class MovingPlatform : PlatformBase
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _distance = 3f;

    private Vector3 _startPos;
    private int _direction = 1;

    public override void ResetPlatform()
    {
        _startPos = transform.position;
        
        _direction = 1;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * _direction * _speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - _startPos.x) >= _distance)
            _direction *= -1;
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
