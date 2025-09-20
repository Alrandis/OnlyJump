using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;      // Игрок
    [SerializeField] private float smoothSpeed = 5f; // Скорость сглаживания
    [SerializeField] private float yOffset = 1f;    // Порог по Y (смещение)
    private Camera cam;
       

    private float fixedX; // фиксируем X при старте

    private void Start()
    {
        fixedX = transform.position.x; // запоминаем X камеры

        cam = Camera.main;
        // Цель: ширина камеры = фиксированное значение, высота подстраивается под экран
        float targetWidth = 6f; // желаемая ширина мира
        float targetHeight = targetWidth * Screen.height / Screen.width;

        cam.orthographicSize = targetHeight / 2f;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        // Камера двигается только вверх, если игрок превысил порог
        if (target.position.y > currentPos.y + yOffset)
        {
            targetPos.y = target.position.y - yOffset;
        }
        // если игрок ниже нижней границы окна
        else if (target.position.y < currentPos.y - yOffset)
        {
            targetPos.y = target.position.y + yOffset;
        }

        // Плавное движение камеры по Y
        transform.position = Vector3.Lerp(currentPos, targetPos, smoothSpeed * Time.deltaTime);

        // Фиксируем X (камера не двигается по горизонтали)
        transform.position = new Vector3(fixedX, transform.position.y, currentPos.z);
    }
}
