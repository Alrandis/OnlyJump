using UnityEngine;

public class EnemyMovement : MonsterBase
{
    private void Update()
    {
        Tick(); // теперь каждый враг двигается автоматически
    }

    public override void ResetMonster()
    {

    }

    public virtual void Tick() { }
}
