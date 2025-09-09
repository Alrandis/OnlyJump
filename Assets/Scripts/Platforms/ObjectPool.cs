using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject Prefab;
    public int InitialSize = 10;
    public bool Expandable = true;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        // Заполняем пул начальными объектами
        for (int i = 0; i < InitialSize; i++)
        {
            CreateObject();
        }
    }

    private GameObject CreateObject()
    {
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
    /// Выдаёт объект из пула
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
        obj.SetActive(true);

        var platform = obj.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.ResetPlatform();
        }

        return obj;
    }

    /// <summary>
    /// Возвращает объект в пул
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        var platform = obj.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.ResetPlatform();
        }

        pool.Enqueue(obj);
    }
}
