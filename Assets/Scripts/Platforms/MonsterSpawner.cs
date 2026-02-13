using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster prefabs")]
    public GameObject StandingMonsterPrefab;
    public GameObject WalkingMonsterPrefab;
    public GameObject ShootingMonsterPrefab;
    public GameObject FlyingMonsterPrefab;
    public GameObject FrogMonsterPrefab;
    public GameObject HeartPrefab;

    [Header("Settings")]
    public float HorizontalOffset = 3f;     // права€ стена
    public float FlyingOffsetY = 2f;
    public float ShootingOffsetY = 3f;

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float SpawnChancePerPlatform = 0.35f;

    [Range(0f, 1f)]
    public float HeartSpawnChance = 0.05f;

    private Dictionary<GameObject, List<GameObject>> _platformMonsters =
        new Dictionary<GameObject, List<GameObject>>();

    // =========================================================
    // ===================== MAIN SPAWN ========================
    // =========================================================

    public void SpawnForPlatform(GameObject platformObj, bool isShort)
    {
        if (platformObj == null) return;
        if (_platformMonsters.ContainsKey(platformObj)) return;

        float platformY = platformObj.transform.position.y;

        bool isVertical = platformObj.GetComponent<VerticalPlatform>() != null;
        bool isSpike = platformObj.GetComponentInChildren<SpikeDamage>() != null;
        bool isNormal = platformObj.GetComponent<NormalPlatform>() != null;
        bool isDisappearing = platformObj.GetComponent<DisappearingPlatform>() != null;

        // ================= HEART =================
        if (!isVertical && Random.value < HeartSpawnChance)
        {
            SpawnHeart(platformObj);
            return;
        }

        // ================= MONSTER CHANCE =================
        if (Random.value > SpawnChancePerPlatform)
            return;

        GameObject prefab = ChooseMonsterByHeight(platformY);

        if (!CanSpawnMonsterOnPlatform(prefab,
            isNormal, isSpike, isShort, isVertical, isDisappearing))
            return;

        List<GameObject> spawned = new List<GameObject>();

        SpawnByType(prefab, platformObj, platformY, spawned);

        if (spawned.Count > 0)
            _platformMonsters[platformObj] = spawned;
    }

    // =========================================================
    // =============== HEIGHT DISTRIBUTION =====================
    // =========================================================

    private GameObject ChooseMonsterByHeight(float y)
    {
        float r = Random.value;

        // 0Ц50
        if (y < 50f)
            return StandingMonsterPrefab;

        // 50Ц100
        if (y < 100f)
            return r < 0.7f ? StandingMonsterPrefab : WalkingMonsterPrefab;

        // 100Ц150
        if (y < 150f)
        {
            if (r < 0.5f) return StandingMonsterPrefab;
            if (r < 0.85f) return WalkingMonsterPrefab;
            return FrogMonsterPrefab;
        }

        // 150Ц250
        if (y < 250f)
        {
            if (r < 0.15f) return StandingMonsterPrefab;
            if (r < 0.45f) return WalkingMonsterPrefab;
            if (r < 0.75f) return FrogMonsterPrefab;
            return ShootingMonsterPrefab;
        }

        // 250Ц400
        if (y < 400f)
        {
            if (r < 0.15f) return WalkingMonsterPrefab;
            if (r < 0.40f) return FrogMonsterPrefab;
            if (r < 0.70f) return ShootingMonsterPrefab;
            return FlyingMonsterPrefab;
        }

        // 400Ц600
        if (y < 600f)
        {
            if (r < 0.10f) return WalkingMonsterPrefab;
            if (r < 0.30f) return FrogMonsterPrefab;
            if (r < 0.65f) return ShootingMonsterPrefab;
            return FlyingMonsterPrefab;
        }

        // 600+ Ч все доступны
        if (r < 0.2f) return StandingMonsterPrefab;
        if (r < 0.4f) return WalkingMonsterPrefab;
        if (r < 0.6f) return FrogMonsterPrefab;
        if (r < 0.8f) return ShootingMonsterPrefab;
        return FlyingMonsterPrefab;
    }

    // =========================================================
    // =============== PLATFORM RULES ==========================
    // =========================================================
    private bool CanSpawnMonsterOnPlatform(
        GameObject monsterPrefab,
        bool isNormal,
        bool isSpike,
        bool isShort,
        bool isVertical,
        bool isDisappearing)
    {
        // Flying можно даже на vertical
        if (monsterPrefab == FlyingMonsterPrefab)
            return true;

        if (isVertical)
            return false;

        if (monsterPrefab == ShootingMonsterPrefab)
            return true;

        if (monsterPrefab == FrogMonsterPrefab)
            return isNormal;

        if (monsterPrefab == WalkingMonsterPrefab)
            return isNormal || isDisappearing;

        if (monsterPrefab == StandingMonsterPrefab)
            return isNormal;

        return false;
    }

    // =========================================================
    // ================= SPAWN POSITIONS =======================
    // =========================================================

    private void SpawnByType(GameObject prefab,
                         GameObject platform,
                         float platformY,
                         List<GameObject> list)
    {
        if (prefab == null) return;

        Vector3 spawnPos;
        Quaternion rotation = prefab.transform.rotation;

        // ================= FLYING =================
        if (prefab == FlyingMonsterPrefab)
        {
            spawnPos = new Vector3(
                0f,
                platformY + FlyingOffsetY,
                0f);
        }

        // ================= SHOOTING =================
        else if (prefab == ShootingMonsterPrefab)
        {
            float platformX = platform.transform.position.x;

            // если платформа слева Ч монстр справа
            float wallX = platformX < 0f ? HorizontalOffset : -HorizontalOffset;

            spawnPos = new Vector3(
                wallX,
                platformY + ShootingOffsetY,
                0f);
        }

        // ================= OTHERS =================
        else
        {
            spawnPos = platform.transform.position + Vector3.up * 0.5f;
        }

        GameObject monster = Spawn(prefab, spawnPos, list);

        // ---------- FLIP SHOOTING ----------
        if (monster != null && prefab == ShootingMonsterPrefab)
        {
            bool spawnedOnLeftWall = monster.transform.position.x < 0f;

            Vector3 scale = monster.transform.localScale;

            // если на левой стене должен смотреть вправо
            if (spawnedOnLeftWall)
                scale.x = -Mathf.Abs(scale.x);
            else
                scale.x = Mathf.Abs(scale.x);

            monster.transform.localScale = scale;
        }
    }


    private GameObject Spawn(GameObject prefab, Vector3 pos, List<GameObject> list)
    {
        GameObject monster = null;

        if (PoolManager.Instance != null)
            monster = PoolManager.Instance.GetObject(prefab, pos, prefab.transform.rotation);

        if (monster == null)
            monster = Instantiate(prefab, pos, prefab.transform.rotation);

        monster.SetActive(true);

        var baseComp = monster.GetComponent<MonsterBase>();
        if (baseComp != null)
        {
            if (baseComp.Pool == null)
                baseComp.Init(null);

            baseComp.Activate();
        }

        list.Add(monster);
        return monster;
    }


    private void SpawnHeart(GameObject platform)
    {
        if (HeartPrefab == null) return;

        Vector3 pos = platform.transform.position + Vector3.up * 0.7f;

        GameObject heart = null;

        if (PoolManager.Instance != null)
            heart = PoolManager.Instance.GetObject(HeartPrefab, pos, HeartPrefab.transform.rotation);

        if (heart == null)
            heart = Instantiate(HeartPrefab, pos, HeartPrefab.transform.rotation);

        heart.SetActive(true);
    }

    // =========================================================
    // ================= CLEANUP ===============================
    // =========================================================

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
