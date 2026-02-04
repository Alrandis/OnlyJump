using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAirControl : MonoBehaviour
{
    [SerializeField] private float _airMoveSpeed = 5f;  

    private Rigidbody2D _rb;

    private bool _isBouncingVertically;   
    private bool _isKnockedBack;          
    private Vector2 _knockbackVelocity;

    private Animator _animator;
    [SerializeField] private PlayerDataSO _playerData;
    public bool IsInAirControl => _isBouncingVertically || _isKnockedBack;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

    }

    private void FixedUpdate()
    {
        if (_isBouncingVertically || _isKnockedBack)
        {
            float inputX = 0f;

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = Input.mousePosition;

                inputX = mousePos.x < Screen.width / 2 ? -1f : 1f;
            }

            _rb.linearVelocity = new Vector2(
                inputX * _airMoveSpeed + _knockbackVelocity.x,
                _knockbackVelocity.y + _rb.linearVelocity.y
            );
        }
    }

    public void Bounce(float bounceForce)
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, bounceForce);
        _isBouncingVertically = true;
        _isKnockedBack = false;
        _knockbackVelocity = Vector2.zero;

        if (_animator != null)
            _animator.SetTrigger("Hurt");
    }


    public void Knockback(Vector2 knockbackImpulse)
    {
        _rb.linearVelocity = knockbackImpulse;

        _playerData.IsKnockedBack = true;
        _playerData.IsJumping = false;

        _isKnockedBack = true;
        _isBouncingVertically = false;

        _knockbackVelocity = new Vector2(_rb.linearVelocity.x, 0);
        if (_animator != null)
            _animator.SetTrigger("Hurt");
    }


    public void ResetAirControl()
    {
        _isBouncingVertically = false;
        _isKnockedBack = false;

        _playerData.IsKnockedBack = false;

        _knockbackVelocity = Vector2.zero;
    }
}
