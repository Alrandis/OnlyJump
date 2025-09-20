using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;      // Игрок
    [SerializeField] private float _smoothSpeed = 5f; // Скорость сглаживания
    [SerializeField] private float _yOffset = 1f;    // Порог по Y (смещение)
    private Camera _cam;
       

    private float _fixedX; // фиксируем X при старте

    private void Start()
    {
        _fixedX = transform.position.x; // запоминаем X камеры

        _cam = Camera.main;
        // Цель: ширина камеры = фиксированное значение, высота подстраивается под экран
        float targetWidth = 6f; // желаемая ширина мира
        float targetHeight = targetWidth * Screen.height / Screen.width;

        _cam.orthographicSize = targetHeight / 2f;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // Камера двигается только вверх, если игрок превысил порог
        if (_target.position.y > currentPos.y + _yOffset)
        {
            targetPos.y = _target.position.y - _yOffset;
        }
        // если игрок ниже нижней границы окна
        else if (_target.position.y < currentPos.y - _yOffset)
        {
            targetPos.y = _target.position.y + _yOffset;
        }

        // Плавное движение камеры по Y
        transform.position = Vector3.Lerp(currentPos, targetPos, _smoothSpeed * Time.deltaTime);

        // Фиксируем X (камера не двигается по горизонтали)
        transform.position = new Vector3(_fixedX, transform.position.y, currentPos.z);
    }
}
