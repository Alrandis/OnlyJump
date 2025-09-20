using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster prefabs")]
    public GameObject StandingMonsterPrefab;
    public GameObject WalkingMonsterPrefab;
    public GameObject ShootingMonsterPrefab;
    public GameObject FlyingMonsterPrefab;

    [Header("Settings")]
    public float HorizontalOffset = 3f;
    public float FlyingOffsetY = 2f;
    public float ShootingOffsetY = 3f;
    public int MaxMonstersPerPlatform = 1; // можно 1-2

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float SpawnChancePerPlatform = 0.3f; // шанс, что на платформе будет враг

    // Словарь платформ -> список монстров на ней
    private Dictionary<GameObject, List<GameObject>> _platformMonsters = new Dictionary<GameObject, List<GameObject>>();

    public void SpawnForPlatform(GameObject platformObj)
    {
        if (platformObj == null) return;
        if (_platformMonsters.ContainsKey(platformObj)) return; // уже спавнили монстра
        if (!CanSpawnOnPlatform(platformObj)) return;

        // Шанс спавна
        if (Random.value > SpawnChancePerPlatform) return; // пропускаем платформу

        float platformY = platformObj.transform.position.y;
        List<GameObject> spawnedMonsters = new List<GameObject>();

        // --- Спавн по высоте ---
        if (platformY >= 20f && platformY < 40f)
        {
            SpawnMonsterOnPlatform(StandingMonsterPrefab, platformObj, Vector3.up * 0.5f, spawnedMonsters);
        }
        else if (platformY >= 40f && platformY < 60f)
        {
            SpawnMonsterOnPlatform(WalkingMonsterPrefab, platformObj, Vector3.up * 0.5f, spawnedMonsters);
        }
        else if (platformY >= 60f && platformY < 80f)
        {
            float wallX = HorizontalOffset; // правая стена
            Vector3 pos = new Vector3(wallX, platformY + ShootingOffsetY, 0f);
            SpawnMonsterOnPlatform(ShootingMonsterPrefab, platformObj, pos - platformObj.transform.position, spawnedMonsters);
        }
        else if (platformY >= 80f)
        {
            Vector3 pos = new Vector3(0f, platformY + FlyingOffsetY, 0f);
            SpawnMonsterOnPlatform(FlyingMonsterPrefab, platformObj, pos + new Vector3(0, 1.2f, 0) - platformObj.transform.position , spawnedMonsters);
        }

        if (spawnedMonsters.Count > 0)
            _platformMonsters[platformObj] = spawnedMonsters;
    }


    private void SpawnMonsterOnPlatform(GameObject prefab, GameObject platform, Vector3 offset, List<GameObject> spawnedList)
    {
        if (prefab == null || spawnedList.Count >= MaxMonstersPerPlatform) return;

        Vector3 spawnPos = platform.transform.position + offset;

        GameObject monster = null;

        if (PoolManager.Instance != null)
            monster = PoolManager.Instance.GetObject(prefab, spawnPos, prefab.transform.rotation);

        if (monster == null)
            monster = Instantiate(prefab, spawnPos, prefab.transform.rotation);

        monster.SetActive(true);

        var monsterBase = monster.GetComponent<MonsterBase>();
        if (monsterBase != null)
        {
            if (monsterBase.Pool == null)
                monsterBase.Init(null); // если пул не передан, хотя бы инициализируем

            monsterBase.Activate();
        }

        spawnedList.Add(monster);
    }

    private bool CanSpawnOnPlatform(GameObject platformObj)
    {
        if (platformObj == null) return false;

        // Если есть компонент SpikePlatform — нельзя спавнить
        return platformObj.GetComponentInChildren<SpikeDamage>() == null;
    }

    // --- Вызывается при очистке платформ ---
    public void RemovePlatform(GameObject platform)
    {
        if (_platformMonsters.TryGetValue(platform, out var monsters))
        {
            foreach (var m in monsters)
            {
                if (m != null)
                    PoolManager.Instance.ReturnObject(m);
            }
            _platformMonsters.Remove(platform);
        }
    }
}
