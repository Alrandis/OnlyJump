using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    [System.Serializable]
    public class PoolPrefab
    {
        public GameObject Prefab;
        public int InitialSize = 5;
        public bool Expandable = true;
    }

    [SerializeField] private List<PoolPrefab> _prefabsToPool;

    private Dictionary<GameObject, ObjectPool> _pools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        foreach (var item in _prefabsToPool)
        {
            GameObject poolObj = new GameObject(item.Prefab.name + "_Pool");
            poolObj.transform.SetParent(transform);
            var pool = poolObj.AddComponent<ObjectPool>();

            pool.Prefab = item.Prefab;
            pool.InitialSize = item.InitialSize;
            pool.Expandable = item.Expandable;

            pool.InitializePool();
            _pools[item.Prefab] = pool;
        }
    }

    /// <summary>
    /// Выдать объект из пула
    /// <summary> 
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(prefab, out var pool))
        {
            Debug.LogWarning($"Нет пула для префаба {prefab.name}");
            return null;
        }

        var obj = pool.GetObject(position, rotation);
        if (obj == null) return null;

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // Автоинициализация для платформ
        if (obj.TryGetComponent<PlatformBase>(out var platform) && platform.Pool == null)
            platform.Init(pool);

        // Автоинициализация для монстров
        if (obj.TryGetComponent<MonsterBase>(out var monster) && monster.Pool == null)
            monster.Init(pool);

        return obj;
    }

    /// <summary>
    /// Вернуть объект в пул
    /// <summary>
    public void ReturnObject(GameObject obj)
    {
        if (obj.TryGetComponent<PlatformBase>(out var platform))
        {
            platform.ResetPlatform();
            platform.Deactivate(); 
            platform.Pool?.ReturnObject(obj);
            return;
        }

        if (obj.TryGetComponent<MonsterBase>(out var monster))
        {
            monster.ResetMonster();
            monster.Deactivate(); 
            monster.Pool?.ReturnObject(obj);
            return;
        }

        obj.SetActive(false);
    }
}
