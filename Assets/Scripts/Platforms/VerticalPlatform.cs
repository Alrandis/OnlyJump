using UnityEngine;

public class VerticalPlatform : PlatformBase
{
    public enum WallSide { Left = -1, Right = 1 }

    [SerializeField] private WallSide side = WallSide.Left;
    [SerializeField] private float distanceFromWall = 2.4f;

    private Vector3 startPos;

    protected override void OnInit()
    {
        // Здесь можно инициализировать анимации или эффекты
    }

    public override void ResetPlatform()
    {
        // Позицию обновим в LevelGenerator
        startPos = transform.position;
    }

    // Метод, который вызываем при спавне, чтобы установить позицию на нужной стене
    public void SetWallSide(WallSide wallSide, float yPos)
    {
        side = wallSide;
        float xPos = (int)side * distanceFromWall;
        transform.position = new Vector3(xPos, yPos, 0f);
        startPos = transform.position;
    }
}
