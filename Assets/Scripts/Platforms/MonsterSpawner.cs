using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject standingMonsterPrefab;
    public GameObject walkingMonsterPrefab;
    public GameObject flyingMonsterPrefab;
    public GameObject shootingMonsterPrefab;

    [Header("Settings")]
    public float horizontalOffset = 3f;   // для стрелков
    public float flyingOffsetY = 2f;      // высота летающего над платформой
    public float shootingOffsetY = 3f;    // высота стрелка над платформой
    public float maxPlayerHeight = 100f;  // для коэффициента сложности
    public int maxMonstersPerPlatform = 2;

    [SerializeField] private Transform player;

    public void SpawnForPlatform(GameObject platform)
    {
        if (player == null) return;

        float t = Mathf.Clamp01(player.position.y / maxPlayerHeight);
        int spawned = 0;

        // --- 1. Стоячий / бродячий ---
        if (platform.CompareTag("VerticalPlatform") || platform.CompareTag("SpikePlatform"))
        {
            // на этих платформах не спавним наземных
        }
        else
        {
            if (spawned < maxMonstersPerPlatform && Random.value < Mathf.Lerp(0.2f, 0.7f, t))
            {
                SpawnMonster(standingMonsterPrefab, platform.transform.position + Vector3.up * 0.5f);
                spawned++;
            }

            if (spawned < maxMonstersPerPlatform && Random.value < Mathf.Lerp(0.2f, 0.6f, t))
            {
                SpawnMonster(walkingMonsterPrefab, platform.transform.position + Vector3.up * 0.5f);
                spawned++;
            }
        }

        // --- 2. Летающий ---
        if (spawned < maxMonstersPerPlatform && Random.value < Mathf.Lerp(0.1f, 0.5f, t))
        {
            Vector3 pos = new Vector3(0, platform.transform.position.y + flyingOffsetY, 0);
            SpawnMonster(flyingMonsterPrefab, pos);
            spawned++;
        }

        // --- 3. Стрелок ---
        if (spawned < maxMonstersPerPlatform && Random.value < Mathf.Lerp(0.1f, 0.4f, t))
        {
            float wallX = Random.value < 0.5f ? -horizontalOffset : horizontalOffset;
            Vector3 pos = new Vector3(wallX, platform.transform.position.y + shootingOffsetY, 0);
            SpawnMonster(shootingMonsterPrefab, pos);
            spawned++;
        }
    }

    private void SpawnMonster(GameObject prefab, Vector3 position)
    {
        if (prefab == null) return;

        GameObject monster = null;

        // Если есть пул — берём из него
        if (PoolManager.Instance != null)
            monster = PoolManager.Instance.GetObject(prefab, position, Quaternion.identity);

        // Если не нашли в пуле — создаём напрямую
        if (monster == null)
            monster = Instantiate(prefab, position, Quaternion.identity);

        monster.transform.position = position;
        monster.SetActive(true);
    }
}
