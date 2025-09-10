using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject prefab;
        public int initialSize = 5;
        public bool expandable = true;
    }

    [SerializeField] private List<PoolPrefab> prefabsToPool;

    private Dictionary<GameObject, ObjectPool> pools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Создаём ObjectPool для каждого префаба
        foreach (var item in prefabsToPool)
        {
            GameObject poolObj = new GameObject(item.prefab.name + "_Pool");
            poolObj.transform.SetParent(transform);
            var pool = poolObj.AddComponent<ObjectPool>();

            pool.Prefab = item.prefab;
            pool.InitialSize = item.initialSize;
            pool.Expandable = item.expandable;

            pools[item.prefab] = pool;
        }
    }

    /// <summary>
    /// Выдать объект нужного типа из пула
    /// </summary>
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(prefab, out var pool))
        {
            Debug.LogWarning($"Нет пула для префаба {prefab.name}");
            return null;
        }
        return pool.GetObject(position, rotation);
    }

    /// <summary>
    /// Вернуть объект в пул
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        var platform = obj.GetComponent<PlatformBase>();
        if (platform != null)
        {
            platform.ResetPlatform();
            platform.gameObject.SetActive(false);
            // используем пул, к которому привязан объект
            platform.Pool?.ReturnObject(obj);
        }
        else
        {
            obj.SetActive(false);
        }
    }
}
