using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Generation Settings")]
    public float platformSpacingY = 2f;
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
            VerticalPlatform.WallSide side = (Random.value < 0.5f) ? VerticalPlatform.WallSide.Left : VerticalPlatform.WallSide.Right;
            vertical.SetWallSide(side, lastPlatformY + platformSpacingY);
        }

        lastPlatformY += platformSpacingY;
    }

    private float ChooseXSlot(GameObject prefab)
    {
        if (prefab == flyingPlatform) return 0f; // летающая по центру
        return Random.value < 0.5f ? -2f : 2f;    // обычные слева/справа
    }

    private GameObject ChoosePlatformPrefab(float height)
    {
        if (height < 20f) return normalPlatform;
        else if (height < 40f) return Random.value < 0.5f ? normalPlatform : disappearingPlatform;
        else if (height < 60f)
        {
            float r = Random.value;
            if (r < 0.33f) return normalPlatform;
            if (r < 0.66f) return disappearingPlatform;
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
            if (activePlatforms[i].transform.position.y < player.position.y - maxFallY)
            {
                PoolManager.Instance.ReturnObject(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }
}
