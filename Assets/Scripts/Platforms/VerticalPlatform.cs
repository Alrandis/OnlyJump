using UnityEngine;

public class VerticalPlatform : PlatformBase
{
    public enum WallSide { Left = -1, Right = 1 }

    [SerializeField] private WallSide side = WallSide.Left;
    [SerializeField] private float distanceFromWall = 2.4f;

    private Vector3 startPos;

    protected override void OnInit()
    {
        // ����� ����� ���������������� �������� ��� �������
    }

    public override void ResetPlatform()
    {
        // ������� ������� � LevelGenerator
        startPos = transform.position;
    }

    // �����, ������� �������� ��� ������, ����� ���������� ������� �� ������ �����
    public void SetWallSide(WallSide wallSide, float yPos)
    {
        side = wallSide;
        float xPos = (int)side * distanceFromWall;
        transform.position = new Vector3(xPos, yPos, 0f);
        startPos = transform.position;
    }
}
