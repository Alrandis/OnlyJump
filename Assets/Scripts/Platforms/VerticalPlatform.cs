using UnityEngine;

public class VerticalPlatform : PlatformBase
{
    public enum WallSide { Left = -1, Right = 1 }

    [SerializeField] private WallSide _side = WallSide.Left;
    [SerializeField] private float _distanceFromWall = 2.4f;

    private Vector3 _startPos;

    //protected override void OnInit()
    //{
        
    //}

    public override void ResetPlatform()
    {
        // Позицию обновим в LevelGenerator
        _startPos = transform.position;
    }

    // Метод, который вызываем при спавне, чтобы установить позицию на нужной стене
    public void SetWallSide(WallSide wallSide, float yPos)
    {
        _side = wallSide;
        float xPos = (int)_side * _distanceFromWall;
        transform.position = new Vector3(xPos, yPos, 0f);
        _startPos = transform.position;
    }
}
