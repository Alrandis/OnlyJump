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

        // ВСЕГДА следуем за игроком с оффсетом
        Vector3 targetPos = new Vector3(
            _fixedX,
            _target.position.y - _yOffset,
            currentPos.z
        );

        transform.position = Vector3.Lerp(
            currentPos,
            targetPos,
            _smoothSpeed * Time.deltaTime
        );
    }

}
