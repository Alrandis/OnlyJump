using UnityEngine;

public class EnemyMovement : MonsterBase
{
    private void Update()
    {
        Tick(); // ������ ������ ���� ��������� �������������
    }

    public override void ResetMonster()
    {

    }

    public virtual void Tick() { }
}
