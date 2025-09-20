using UnityEngine;

public class WallsFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _leftWall;
    [SerializeField] private Transform _rightWall;

    [SerializeField] private float _verticalOffset = 5f; // запас сверху
    [SerializeField] private float _moveSpeed = 3f;      // скорость плавного движения

    private void Update()
    {
        // целевая позиция Y всегда = позиция игрока + offset
        float targetY = _player.position.y + _verticalOffset;

        // обновляем позиции стен
        Vector3 leftTarget = new Vector3(_leftWall.position.x, targetY, _leftWall.position.z);
        Vector3 rightTarget = new Vector3(_rightWall.position.x, targetY, _rightWall.position.z);

        // плавно тянем стены
        _leftWall.position = Vector3.Lerp(_leftWall.position, leftTarget, _moveSpeed * Time.deltaTime);
        _rightWall.position = Vector3.Lerp(_rightWall.position, rightTarget, _moveSpeed * Time.deltaTime);
    }
}
