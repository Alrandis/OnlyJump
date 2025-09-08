using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour
{
    private void Update()
    {
        Tick(); // теперь каждый враг двигается автоматически
    }

    public abstract void Tick();
}
