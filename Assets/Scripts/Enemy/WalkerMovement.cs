using UnityEngine;

public class WalkerMovement : EnemyMovement
{
    public float Speed = 2f;
    public Transform GroundCheck;
    public LayerMask GroundLayer;

    private bool _movingRight = true;

    public override void Tick()
    {
        transform.Translate(Vector2.right * (_movingRight ? 1 : -1) * Speed * Time.deltaTime);

        // Проверка, есть ли земля впереди
        RaycastHit2D groundInfo = Physics2D.Raycast(GroundCheck.position, Vector2.down, 1f, GroundLayer);
        if (!groundInfo.collider)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _movingRight = !_movingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
