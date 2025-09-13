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
        float xPos = ChooseXSlot(lastPlatformY);
        GameObject prefab = ChoosePlatformPrefab(lastPlatformY);

        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(xPos, lastPlatformY + platformSpacingY, 0f);
        GameObject platformObj = PoolManager.Instance.GetObject(prefab, spawnPos, Quaternion.identity);
        activePlatforms.Add(platformObj);

        // Настраиваем исчезновение для FallingPlatform
        var fallingPlatform = platformObj.GetComponent<FallingPlatform>();
        if (fallingPlatform != null)
        {
            fallingPlatform.UpdateDisappearDelay(player.position.y);
        }

        lastPlatformY += platformSpacingY;
    }

    private float ChooseXSlot(float height)
    {
        // Летающая платформа всегда по центру
        if (height > 60f) return 0f;

        return Random.value < 0.5f ? -2f : 2f;
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
            float r = Random.value;
            if (r < 0.25f) return normalPlatform;
            if (r < 0.5f) return disappearingPlatform;
            if (r < 0.75f) return spikesPlatform;
            return flyingPlatform;
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
