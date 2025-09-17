using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Generation Settings")]
    public float platformSpacingY = 2f;
    public float platformSpacingX = 1.5f;
    public int platformsBuffer = 5;
    public float maxFallY = 10f;

    [Header("Platform Prefabs")]
    public GameObject normalPlatform;
    public GameObject disappearingPlatform;
    public GameObject spikesPlatform;
    public GameObject flyingPlatform;
    public GameObject verticalPlatform;

    private List<GameObject> activePlatforms = new List<GameObject>();
    private float lastPlatformY = 0f;

    [Header("Monster Spawner")]
    public MonsterSpawner monsterSpawner;

    private VerticalPlatform.WallSide? lastVerticalSide = null;

    private void Start()
    {
        lastPlatformY = player.position.y - platformSpacingY;

        for (int i = 0; i < platformsBuffer; i++)
        {
            GenerateNextPlatform();
        }
    }

    private void Update()
    {
        while (player.position.y + platformsBuffer * platformSpacingY > lastPlatformY)
        {
            GenerateNextPlatform();
        }

        CleanupPlatforms();
    }

    private void GenerateNextPlatform()
    {
        GameObject prefab = ChoosePlatformPrefab(lastPlatformY);
        float xPos = ChooseXSlot(prefab);

        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(xPos, lastPlatformY + platformSpacingY, 0f);
        GameObject platformObj = PoolManager.Instance.GetObject(prefab, spawnPos, prefab.transform.rotation);
        activePlatforms.Add(platformObj);

        // Настраиваем исчезновение для FallingPlatform
        var fallingPlatform = platformObj.GetComponent<FallingPlatform>();
        if (fallingPlatform != null)
        {
            fallingPlatform.UpdateDisappearDelay(player.position.y);
        }

        // Если это вертикальная платформа
        var vertical = platformObj.GetComponent<VerticalPlatform>();
        if (vertical != null)
        {
            VerticalPlatform.WallSide side;

            // если предыдущая вертикальная была, ставим на другую сторону
            if (lastVerticalSide.HasValue)
                side = lastVerticalSide.Value == VerticalPlatform.WallSide.Left
                       ? VerticalPlatform.WallSide.Right
                       : VerticalPlatform.WallSide.Left;
            else
                side = (Random.value < 0.5f) ? VerticalPlatform.WallSide.Left : VerticalPlatform.WallSide.Right;

            vertical.SetWallSide(side, lastPlatformY + platformSpacingY);
            lastVerticalSide = side; // запоминаем для следующей вертикальной платформы
        }

        // --- ВЫЗОВ SPAWN FOR PLATFORM (тандем с генерацией платформы) ---
        if (monsterSpawner != null)
        {
            monsterSpawner.SpawnForPlatform(platformObj);
        }

        lastPlatformY += platformSpacingY;
    }

    private float ChooseXSlot(GameObject prefab)
    {
        if (prefab == flyingPlatform) return 0f; // летающая по центру
        return Random.value < 0.5f ? -platformSpacingX : platformSpacingX;    // обычные слева/справа
    }

    private GameObject ChoosePlatformPrefab(float height)
    {
        if (height < 20f) return normalPlatform;
        else if (height < 50f) return Random.value < 0.4f ? normalPlatform : disappearingPlatform;
        else if (height < 80f)
        {
            float r = Random.value;
            if (r < 0.30f) return normalPlatform;
            if (r < 0.80f) return disappearingPlatform;
            return spikesPlatform;
        }
        else
        {
            float r2 = Random.value;
            if (r2 < 0.2f) return normalPlatform;
            if (r2 < 0.4f) return disappearingPlatform;
            if (r2 < 0.6f) return spikesPlatform;
            if (r2 < 0.8f) return flyingPlatform;
            return verticalPlatform; // новая вертикальная платформа
        }
    }

    private void CleanupPlatforms()
    {
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            var platform = activePlatforms[i];
            var falling = platform.GetComponent<FallingPlatform>();

            if (platform.transform.position.y < player.position.y - maxFallY
                || (falling != null && falling.MarkedForRemoval))
            {
                // перед возвратом сбрасываем состояние
                falling?.ResetPlatform();

                PoolManager.Instance.ReturnObject(platform);
                monsterSpawner?.RemovePlatform(platform);
                activePlatforms.RemoveAt(i);
            }
        }
    }

}
