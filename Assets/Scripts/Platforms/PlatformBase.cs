using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    protected ObjectPool _pool;

    public void Init(ObjectPool pool)
    {
        _pool = pool;
        OnInit();
    }

    // Каждый тип платформы сбрасывает свое состояние по-своему
    public abstract void ResetPlatform();

    protected void ReturnToPool()
    {
        if (_pool != null)
            _pool.ReturnObject(gameObject);
        else
            gameObject.SetActive(false);
    }

    // Хук для доп. инициализации (если кому-то нужно)
    protected virtual void OnInit() { }
}
