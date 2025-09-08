using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAirControl : MonoBehaviour
{
    [SerializeField] private float _airMoveSpeed = 5f;  

    private Rigidbody2D _rb;

    private bool _isBouncingVertically;   
    private bool _isKnockedBack;          
    private Vector2 _knockbackVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        
    }


    public void Knockback(Vector2 knockbackImpulse)
    {
        _rb.linearVelocity = knockbackImpulse;
        _isKnockedBack = true;
        _isBouncingVertically = false;
        _knockbackVelocity = new Vector2(_rb.linearVelocity.x, 0);
    }


    public void ResetAirControl()
    {
        _isBouncingVertically = false;
        _isKnockedBack = false;
        _knockbackVelocity = Vector2.zero;
    }
}
