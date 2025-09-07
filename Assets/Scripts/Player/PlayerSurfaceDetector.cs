using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayerSurfaceDetector : MonoBehaviour
{
    [SerializeField] private PlayerDataSO PlayerData;

    [Header("Ground Check")]
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private float GroundRadius = 0.2f;
    [SerializeField] private LayerMask GroundLayer;

    [Header("Wall Check")]
    [SerializeField] private LayerMask WallLayer;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // ѕроверка земли
        PlayerData.IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectWall(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // если игрок оторвалс€ от стены
        if (((1 << collision.gameObject.layer) & WallLayer) != 0)
        {
            PlayerData.IsTouchingWall = false;
        }
    }

    private void DetectWall(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & WallLayer) != 0)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (Mathf.Abs(contact.normal.x) > 0.9f)
                {
                    PlayerData.IsTouchingWall = true;
                   
                    // сбросить горизонтальную скорость при контакте
                    _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);

                    PlayerData.IsJumping = false;
                }
            }
        }
    }

}
