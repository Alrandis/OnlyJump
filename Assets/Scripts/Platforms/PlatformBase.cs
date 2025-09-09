using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    public ObjectPool Pool;

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        OnInit();
    }

    // Каждый тип платформы сбрасывает свое состояние по-своему
    public abstract void ResetPlatform();

    protected void ReturnToPool()
    {
        if (Pool != null)
            Pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }

    // Хук для доп. инициализации (если кому-то нужно)
    protected virtual void OnInit() { }
}
