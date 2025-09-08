using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    private void Update()
    {
        Tick(); // ������ ������ ���� ��������� �������������
    }

    public abstract void Tick();
}
