using UnityEngine;

public abstract class PlatformBase : MonoBehaviour
{
    public ObjectPool Pool { get; private set; }

    // Сохраняем корневой объект платформы
    private GameObject platformRoot;

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        platformRoot = GetRootObject();
        OnInit();
    }

    // Получаем корень платформы
    private GameObject GetRootObject()
    {
        // Если скрипт на корне — вернём себя
        if (transform.parent == null || transform.parent.GetComponent<ObjectPool>() == null)
            return gameObject;

        // Если есть родитель — ищем первый объект выше, который не PoolManager
        Transform t = transform;
        while (t.parent != null && t.parent.GetComponent<ObjectPool>() == null)
            t = t.parent;

        return t.gameObject;
    }

    // Сбрасывает состояние платформы
    public abstract void ResetPlatform();

    // Возврат в пул
    protected void ReturnToPool()
    {
        if (Pool != null)
            Pool.ReturnObject(platformRoot);
        else
            platformRoot.SetActive(false);
    }

    // Хук для дополнительной инициализации
    protected virtual void OnInit() { }

    // Включение платформы
    public void Activate()
    {
        platformRoot.SetActive(true);
    }

    // Деактивация платформы
    public void Deactivate()
    {
        platformRoot.SetActive(false);
    }
}
