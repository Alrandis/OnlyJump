using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Player")]
    public Transform Player;

    [Header("Generation Settings")]
    public float PlatformSpacingY = 1.8f;
    public float PlatformSpacingX = 1.4f;
    public int PlatformsBuffer = 5;
    public float MaxFallY = 10f;

    [Header("Platform Prefabs")]
    public GameObject NormalPlatform;
    public GameObject DisappearingPlatform;
    public GameObject SpikesPlatform;
    public GameObject FlyingPlatform;
    public GameObject VerticalPlatform;

    private List<GameObject> _activePlatforms = new List<GameObject>();
    private float _lastPlatformY = 0f;

    [Header("Monster Spawner")]
    public MonsterSpawner MonsterSpawner;

    private VerticalPlatform.WallSide? _lastVerticalSide = null;

    private void Start()
    {
        _lastPlatformY = Player.position.y - PlatformSpacingY;

        for (int i = 0; i < PlatformsBuffer; i++)
        {
            GenerateNextPlatform();
        }
    }

    private void Update()
    {
        while (Player.position.y + PlatformsBuffer * PlatformSpacingY > _lastPlatformY)
        {
            GenerateNextPlatform();
        }

        CleanupPlatforms();
    }

    private void GenerateNextPlatform()
    {
        GameObject prefab = ChoosePlatformPrefab(_lastPlatformY);
        float xPos = ChooseXSlot(prefab);

        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(xPos, _lastPlatformY + PlatformSpacingY, 0f);
        GameObject platformObj = PoolManager.Instance.GetObject(prefab, spawnPos, prefab.transform.rotation);
        _activePlatforms.Add(platformObj);

        // Настраиваем исчезновение для FallingPlatform
        var fallingPlatform = platformObj.GetComponent<FallingPlatform>();
        if (fallingPlatform != null)
        {
            fallingPlatform.UpdateDisappearDelay(Player.position.y);
        }

        // Если это вертикальная платформа
        var vertical = platformObj.GetComponent<VerticalPlatform>();
        if (vertical != null)
        {
            VerticalPlatform.WallSide side;

            // если предыдущая вертикальная была, ставим на другую сторону
            if (_lastVerticalSide.HasValue)
                side = _lastVerticalSide.Value == global::VerticalPlatform.WallSide.Left
                       ? global::VerticalPlatform.WallSide.Right
                       : global::VerticalPlatform.WallSide.Left;
            else
                side = (Random.value < 0.5f) ? global::VerticalPlatform.WallSide.Left : global::VerticalPlatform.WallSide.Right;

            vertical.SetWallSide(side, _lastPlatformY + PlatformSpacingY);
            _lastVerticalSide = side; // запоминаем для следующей вертикальной платформы
        }

        // --- ВЫЗОВ SPAWN FOR PLATFORM (тандем с генерацией платформы) ---
        if (MonsterSpawner != null)
        {
            MonsterSpawner.SpawnForPlatform(platformObj);
        }

        _lastPlatformY += PlatformSpacingY;
    }

    private float ChooseXSlot(GameObject prefab)
    {
        if (prefab == FlyingPlatform) return 0f; // летающая по центру
        return Random.value < 0.5f ? -PlatformSpacingX : PlatformSpacingX;    // обычные слева/справа
    }

    private GameObject ChoosePlatformPrefab(float height)
    {
        if (height < 40f) return NormalPlatform;
        else if (height < 100f) return Random.value < 0.7f ? NormalPlatform : SpikesPlatform;
        else if (height < 150f)
        {
            float r = Random.value;
            if (r < 0.7f) return DisappearingPlatform;
            return SpikesPlatform;
        }
        else if (height < 200f)
        {
            float r2 = Random.value;
            if (r2 < 0.4f) return DisappearingPlatform;
            return SpikesPlatform;
        }
        else if (height < 250f)
        {
            float r2 = Random.value;
            if (r2 < 0.4f) return DisappearingPlatform;
            return FlyingPlatform;
        }
        else
        {
            float r2 = Random.value;
            if (r2 < 0.4f) return SpikesPlatform;
            return VerticalPlatform; // новая вертикальная платформа
        }
    }

    private void CleanupPlatforms()
    {
        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            var platform = _activePlatforms[i];
            var falling = platform.GetComponent<FallingPlatform>();

            if (platform.transform.position.y < Player.position.y - MaxFallY
                || (falling != null && falling.MarkedForRemoval))
            {
                // перед возвратом сбрасываем состояние
                falling?.ResetPlatform();

                PoolManager.Instance.ReturnObject(platform);
                MonsterSpawner?.RemovePlatform(platform);
                _activePlatforms.RemoveAt(i);
            }
        }
    }

}
