using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJumpController : MonoBehaviour
{
    [SerializeField] private PlayerDataSO PlayerData; // ссылка на ScriptableObject
    private Rigidbody2D _rb;
    private Animator _animator;
    private float _gravity;

    [SerializeField] private Vector2 _jumpTarget;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _gravity = Mathf.Abs(Physics2D.gravity.y * _rb.gravityScale);
    }

    private void Update()
    {
        // Прыжок только если игрок на земле или у стены (данные берём из SO)
        if (Input.GetMouseButtonDown(0) && (PlayerData.IsGrounded || PlayerData.IsTouchingWall))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 target = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            Jump(target);
        }
    }

    private void FixedUpdate()
    {
        if (PlayerData.IsJumping)
        {
            Vector2 currentPos = _rb.position;
            float distance = Vector2.Distance(currentPos, _jumpTarget);

            if (distance < 0.2f)
            {
                // Не меняем как ты просил
                _rb.linearVelocity = new Vector2(_rb.linearVelocity.x * 0.2f, _rb.linearVelocity.y * 0.01f);
                PlayerData.IsJumping = false;
            }
        }

        // обновляем velocity в SO (чтобы другие скрипты знали актуальное состояние)
        PlayerData.CurrentVelocity = _rb.linearVelocity;
    }

    private void Jump(Vector2 target)
    {
        Vector2 startPos = _rb.position;
        Vector2 delta = target - startPos;

        delta.x = Mathf.Clamp(delta.x, -PlayerData.MaxJumpX, PlayerData.MaxJumpX);
        delta.y = Mathf.Clamp(delta.y, -PlayerData.MaxJumpY, PlayerData.MaxJumpY);

        float vx = delta.x / PlayerData.JumpTime;
        float vy = delta.y / PlayerData.JumpTime + 0.5f * _gravity * PlayerData.JumpTime;

        _rb.linearVelocity = new Vector2(vx, vy);

        _jumpTarget = startPos + delta;
        PlayerData.IsJumping = true;

        if (_animator != null)
            _animator.SetTrigger("Jump");
    }
}
