using UnityEngine;

public class WallsFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;

    [SerializeField] private float verticalOffset = 5f; // ����� ������
    [SerializeField] private float moveSpeed = 3f;      // �������� �������� ��������

    private void Update()
    {
        // ������� ������� Y ������ = ������� ������ + offset
        float targetY = player.position.y + verticalOffset;

        // ��������� ������� ����
        Vector3 leftTarget = new Vector3(leftWall.position.x, targetY, leftWall.position.z);
        Vector3 rightTarget = new Vector3(rightWall.position.x, targetY, rightWall.position.z);

        // ������ ����� �����
        leftWall.position = Vector3.Lerp(leftWall.position, leftTarget, moveSpeed * Time.deltaTime);
        rightWall.position = Vector3.Lerp(rightWall.position, rightTarget, moveSpeed * Time.deltaTime);
    }
}
