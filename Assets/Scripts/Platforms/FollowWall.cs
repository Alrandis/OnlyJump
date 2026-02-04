using UnityEngine;

public class FollowWall : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _spriteA;
    [SerializeField] private Transform _spriteB;

    private float _spriteHeight;

    private void Awake()
    {
        _spriteHeight = _spriteA.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void LateUpdate()
    {
        float camY = _camera.position.y;

        float middleY = (_spriteA.position.y + _spriteB.position.y) * 0.5f;

        // Камера ушла вверх
        if (camY > middleY + _spriteHeight / 2f)
        {
            MoveLowestUp();
        }
        // Камера ушла вниз
        else if (camY < middleY - _spriteHeight / 2f)
        {
            MoveHighestDown();
        }
    }

    private void MoveLowestUp()
    {
        Transform lowest = _spriteA.position.y < _spriteB.position.y ? _spriteA : _spriteB;
        Transform highest = lowest == _spriteA ? _spriteB : _spriteA;

        lowest.position = highest.position + Vector3.up * _spriteHeight;
    }

    private void MoveHighestDown()
    {
        Transform highest = _spriteA.position.y > _spriteB.position.y ? _spriteA : _spriteB;
        Transform lowest = highest == _spriteA ? _spriteB : _spriteA;

        highest.position = lowest.position - Vector3.up * _spriteHeight;
    }
}
