using UnityEngine;

[CreateAssetMenu(menuName = "Player/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    [Header("Jump Settings")]
    public float JumpTime = 0.4f;
    public float MaxJumpX = 8f;
    public float MaxJumpY = 4f;

    [Header("Ground Check")]
    public float GroundRadius = 0.2f;
    public LayerMask GroundLayer;

    [Header("Wall Settings")]
    public LayerMask WallLayer;
    public float WallSlideSpeed = 2f;

    [Header("Runtime State")]
    public bool IsGrounded;
    public bool IsTouchingWall;
    public bool IsJumping;
    public Vector2 CurrentVelocity;
    public bool IsKnockedBack;
}
