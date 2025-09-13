using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // �����
    [SerializeField] private float smoothSpeed = 5f; // �������� �����������
    [SerializeField] private float yOffset = 2f;    // ����� �� Y (��������)

    private float fixedX; // ��������� X ��� ������

    private void Start()
    {
        fixedX = transform.position.x; // ���������� X ������
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // ������ ��������� ������ �����, ���� ����� �������� �����
        if (target.position.y > currentPos.y + yOffset)
        {
            targetPos.y = target.position.y - yOffset;
        }
        // ���� ����� ���� ������ ������� ����
        else if (target.position.y < currentPos.y - yOffset)
        {
            targetPos.y = target.position.y + yOffset;
        }

        // ������� �������� ������ �� Y
        transform.position = Vector3.Lerp(currentPos, targetPos, smoothSpeed * Time.deltaTime);

        // ��������� X (������ �� ��������� �� �����������)
        transform.position = new Vector3(fixedX, transform.position.y, currentPos.z);
    }
}
