using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;      // �����
    [SerializeField] private float _smoothSpeed = 5f; // �������� �����������
    [SerializeField] private float _yOffset = 1f;    // ����� �� Y (��������)
    private Camera _cam;
       

    private float _fixedX; // ��������� X ��� ������

    private void Start()
    {
        _fixedX = transform.position.x; // ���������� X ������

        _cam = Camera.main;
        // ����: ������ ������ = ������������� ��������, ������ �������������� ��� �����
        float targetWidth = 6f; // �������� ������ ����
        float targetHeight = targetWidth * Screen.height / Screen.width;

        _cam.orthographicSize = targetHeight / 2f;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // ������ ��������� ������ �����, ���� ����� �������� �����
        if (_target.position.y > currentPos.y + _yOffset)
        {
            targetPos.y = _target.position.y - _yOffset;
        }
        // ���� ����� ���� ������ ������� ����
        else if (_target.position.y < currentPos.y - _yOffset)
        {
            targetPos.y = _target.position.y + _yOffset;
        }

        // ������� �������� ������ �� Y
        transform.position = Vector3.Lerp(currentPos, targetPos, _smoothSpeed * Time.deltaTime);

        // ��������� X (������ �� ��������� �� �����������)
        transform.position = new Vector3(_fixedX, transform.position.y, currentPos.z);
    }
}
