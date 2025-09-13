using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;

    [Header("Generation Settings")]
    public float platformSpacingY = 2f;      // шаг по Y между платформами
    public int platformsBuffer = 5;          // сколько платформ генерим сверху заранее
    public float maxFallY = 10f;             // расстояние вниз, после которого платформы возвращаем в пул

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
        // Инициализируем генерацию: создаём буфер сверху
        lastPlatformY = player.position.y - platformSpacingY; // чтобы первая платформа была чуть ниже
        for (int i = 0; i < platformsBuffer; i++)
        {
            GenerateNextPlatform();
        }
    }

    private void Update()
    {
        // Генерируем новые платформы, если игрок поднимается выше
        while (player.position.y + platformsBuffer * platformSpacingY > lastPlatformY)
        {
            GenerateNextPlatform();
        }

        // Убираем платформы слишком далеко вниз
        CleanupPlatforms();
    }

    private void GenerateNextPlatform()
    {
        // Выбираем X слот
        float xPos = ChooseXSlot();

        // Определяем тип платформы по высоте игрока
        GameObject prefab = ChoosePlatformPrefab(lastPlatformY);

        // Генерим платформу через пул
        Vector3 spawnPos = new Vector3(xPos, lastPlatformY + platformSpacingY, 0f);
        GameObject platform = PoolManager.Instance.GetObject(prefab, spawnPos, Quaternion.identity);
        activePlatforms.Add(platform);

        // Обновляем последнюю высоту
        lastPlatformY += platformSpacingY;
    }

    private float ChooseXSlot()
    {
        // Случайно выбираем левый или правый слот
        // Для летающей платформы всегда по центру (0)
        float rand = Random.value;
        if (rand < 0.5f)
            return -2f; // левый слот, можно потом вынести в настройки
        else
            return 2f;  // правый слот
    }

    private GameObject ChoosePlatformPrefab(float height)
    {
        // Здесь делаем привязку к высоте для сложности
        if (height < 20f)
            return normalPlatform;
        else if (height < 40f)
            return Random.value < 0.5f ? normalPlatform : disappearingPlatform;
        else if (height < 60f)
        {
            float r = Random.value;
            if (r < 0.33f) return normalPlatform;
            if (r < 0.66f) return disappearingPlatform;
            return spikesPlatform;
        }
        else
        {
            // выше 60 — шанс на всё
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
