using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // �����
    [SerializeField] private float smoothSpeed = 5f; // �������� �����������
    [SerializeField] private float yOffset = 1f;    // ����� �� Y (��������)
    private Camera cam;
       

    private float fixedX; // ��������� X ��� ������

    private void Start()
    {
        fixedX = transform.position.x; // ���������� X ������

        cam = Camera.main;
        // ����: ������ ������ = ������������� ��������, ������ �������������� ��� �����
        float targetWidth = 6f; // �������� ������ ����
        float targetHeight = targetWidth * Screen.height / Screen.width;

        cam.orthographicSize = targetHeight / 2f;
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
