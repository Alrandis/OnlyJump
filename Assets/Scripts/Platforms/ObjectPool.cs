using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject Prefab;
    public int InitialSize = 10;
    public bool Expandable = true;

    private Queue<GameObject> pool = new Queue<GameObject>();

    // Инициализация пула вызывается после присвоения Prefab
    public void InitializePool()
    {
        if (Prefab == null)
        {
            Debug.LogError($"{name} Pool has no prefab assigned!");
            return;
        }

        for (int i = 0; i < InitialSize; i++)
        {
            CreateObject();
        }
    }

    private GameObject CreateObject()
    {
        if (Prefab == null)
            return null;

        var obj = Instantiate(Prefab, transform);
        obj.SetActive(false);

        var platform = obj.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.Init(this);
        }

        pool.Enqueue(obj);
        return obj;
    }

    /// <summary>
    /// Получение объекта из пула
    /// </summary>
    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (pool.Count == 0)
        {
            if (Expandable)
            {
                CreateObject();
            }
            else
            {
                Debug.LogWarning("Pool is empty and not expandable!");
                return null;
            }
        }

        var obj = pool.Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // Активируем корень объекта платформы
        var platform = obj.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.ResetPlatform();
            platform.Activate();
        }
        else
        {
            obj.SetActive(true);
        }

        return obj;
    }

    /// <summary>
    /// Возврат объекта в пул
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        var platform = obj.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.ResetPlatform();
            platform.Deactivate();
        }
        else
        {
            obj.SetActive(false);
        }

        pool.Enqueue(obj);
    }
}
