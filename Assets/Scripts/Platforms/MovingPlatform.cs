using UnityEngine;

public class MovingPlatform : PlatformBase
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float distance = 3f;

    private Vector3 startPos;
    private int direction = 1;

    protected override void OnInit()
    {
        startPos = transform.position;
    }

    public override void ResetPlatform()
    {
        transform.position = startPos;
        direction = 1;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startPos.x) >= distance)
            direction *= -1;
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
