using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    public ObjectPool Pool { get; private set; }

    private GameObject monsterRoot;

    public void Init(ObjectPool pool)
    {
        Pool = pool;
        monsterRoot = GetRootObject();
        OnInit();
    }

    private GameObject GetRootObject()
    {
        // Если скрипт на корне — вернём себя
        if (transform.parent == null || transform.parent.GetComponent<ObjectPool>() == null)
            return gameObject;

        // Если есть родитель — ищем первый объект выше, который не ObjectPool
        Transform t = transform;
        while (t.parent != null && t.parent.GetComponent<ObjectPool>() == null)
            t = t.parent;

        return t.gameObject;
    }

    /// <summary>
    /// Сброс состояния монстра при возврате в пул
    /// </summary>
    public abstract void ResetMonster();

    /// <summary>
    /// Возврат в пул
    /// </summary>
    protected void ReturnToPool()
    {
        if (Pool != null)
            Pool.ReturnObject(monsterRoot);
        else
            monsterRoot.SetActive(false);
    }

    /// <summary>
    /// Дополнительная инициализация (override в наследниках)
    /// </summary>
    protected virtual void OnInit() { }

    /// <summary>
    /// Активация монстра (например, при спавне)
    /// </summary>
    public void Activate()
    {
        monsterRoot.SetActive(true);
        ResetMonster();
    }

    /// <summary>
    /// Деактивация монстра (например, при смерти)
    /// </summary>
    public void Deactivate()
    {
        monsterRoot.SetActive(false);
    }
}
