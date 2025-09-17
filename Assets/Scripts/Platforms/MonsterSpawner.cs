using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster prefabs")]
    public GameObject standingMonsterPrefab;
    public GameObject walkingMonsterPrefab;
    public GameObject shootingMonsterPrefab;
    public GameObject flyingMonsterPrefab;

    [Header("Settings")]
    public float horizontalOffset = 3f;
    public float flyingOffsetY = 2f;
    public float shootingOffsetY = 3f;
    public int maxMonstersPerPlatform = 1; // можно 1-2

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float spawnChancePerPlatform = 0.3f; // шанс, что на платформе будет враг

    // Словарь платформ -> список монстров на ней
    private Dictionary<GameObject, List<GameObject>> platformMonsters = new Dictionary<GameObject, List<GameObject>>();

    public void SpawnForPlatform(GameObject platformObj)
    {
        if (platformObj == null) return;
        if (platformMonsters.ContainsKey(platformObj)) return; // уже спавнили монстра
        if (!CanSpawnOnPlatform(platformObj)) return;

        // Шанс спавна
        if (Random.value > spawnChancePerPlatform) return; // пропускаем платформу

        float platformY = platformObj.transform.position.y;
        List<GameObject> spawnedMonsters = new List<GameObject>();

        // --- Спавн по высоте ---
        if (platformY >= 20f && platformY < 40f)
        {
            SpawnMonsterOnPlatform(standingMonsterPrefab, platformObj, Vector3.up * 0.5f, spawnedMonsters);
        }
        else if (platformY >= 40f && platformY < 60f)
        {
            SpawnMonsterOnPlatform(walkingMonsterPrefab, platformObj, Vector3.up * 0.5f, spawnedMonsters);
        }
        else if (platformY >= 60f && platformY < 80f)
        {
            float wallX = horizontalOffset; // правая стена
            Vector3 pos = new Vector3(wallX, platformY + shootingOffsetY, 0f);
            SpawnMonsterOnPlatform(shootingMonsterPrefab, platformObj, pos - platformObj.transform.position, spawnedMonsters);
        }
        else if (platformY >= 80f)
        {
            Vector3 pos = new Vector3(0f, platformY + flyingOffsetY, 0f);
            SpawnMonsterOnPlatform(flyingMonsterPrefab, platformObj, pos - platformObj.transform.position, spawnedMonsters);
        }

        if (spawnedMonsters.Count > 0)
            platformMonsters[platformObj] = spawnedMonsters;
    }


    private void SpawnMonsterOnPlatform(GameObject prefab, GameObject platform, Vector3 offset, List<GameObject> spawnedList)
    {
        if (prefab == null || spawnedList.Count >= maxMonstersPerPlatform) return;

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
        if (platformMonsters.TryGetValue(platform, out var monsters))
        {
            foreach (var m in monsters)
            {
                if (m != null)
                    PoolManager.Instance.ReturnObject(m);
            }
            platformMonsters.Remove(platform);
        }
    }
}
