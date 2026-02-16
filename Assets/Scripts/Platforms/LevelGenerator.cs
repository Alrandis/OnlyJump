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

    public Transform LavaTransform;

    [Header("Platform Prefabs")]
    public GameObject NormalPlatform;
    public GameObject DisappearingPlatform;
    public GameObject SpikesPlatform;
    public GameObject FlyingPlatform;
    public GameObject VerticalPlatform;
    [Header("Short Platforms")]
    public GameObject ShortPlatform;
    public GameObject ShortDisapearPlatform;
    public GameObject ShortSpikePlatform;

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

    private bool IsShortPlatform(GameObject prefab)
    {
        return prefab.GetComponent<IsShort>() != null;
    }

    private void GenerateNextPlatform()
    {
        GameObject prefab = ChoosePlatformPrefab(_lastPlatformY);
        float xPos = ChooseXSlot(prefab);

        if (prefab == null) return;

        Vector3 spawnPos = new Vector3(xPos, _lastPlatformY + PlatformSpacingY, 0f);
        GameObject platformObj = PoolManager.Instance.GetObject(prefab, spawnPos, prefab.transform.rotation);
        _activePlatforms.Add(platformObj);

        // Ќастраиваем исчезновение дл€ FallingPlatform
        var fallingPlatform = platformObj.GetComponent<FallingPlatform>();
        if (fallingPlatform != null)
        {
            fallingPlatform.UpdateDisappearDelay(Player.position.y);
        }

        // ≈сли это вертикальна€ платформа
        var vertical = platformObj.GetComponent<VerticalPlatform>();
        if (vertical != null)
        {
            VerticalPlatform.WallSide side;

            // если предыдуща€ вертикальна€ была, ставим на другую сторону
            if (_lastVerticalSide.HasValue)
                side = _lastVerticalSide.Value == global::VerticalPlatform.WallSide.Left
                       ? global::VerticalPlatform.WallSide.Right
                       : global::VerticalPlatform.WallSide.Left;
            else
                side = (Random.value < 0.5f) ? global::VerticalPlatform.WallSide.Left : global::VerticalPlatform.WallSide.Right;

            vertical.SetWallSide(side, _lastPlatformY + PlatformSpacingY);
            _lastVerticalSide = side; // запоминаем дл€ следующей вертикальной платформы
        }

        // --- ¬џ«ќ¬ SPAWN FOR PLATFORM (тандем с генерацией платформы) ---
        if (MonsterSpawner != null)
        {
            MonsterSpawner.SpawnForPlatform(platformObj, IsShortPlatform(platformObj));
        }

        _lastPlatformY += PlatformSpacingY;
    }

    private float ChooseXSlot(GameObject prefab)
    {
        if (prefab == FlyingPlatform) return 0f; // летающа€ по центру
        return Random.value < 0.5f ? -PlatformSpacingX : PlatformSpacingX;    // обычные слева/справа
    }

    private GameObject ChoosePlatformPrefab(float height)
    {
        float r = Random.value;

        // 0Ц50: только обучение
        if (height < 50f)
        {
            return NormalPlatform;
        }

        // 50Ц100: обычные + короткие
        else if (height < 100f)
        {
            return r < 0.7f ? NormalPlatform : ShortPlatform;
        }

        // 100Ц150: добавл€ютс€ шипы
        else if (height < 150f)
        {
            if (r < 0.5f) return NormalPlatform;
            if (r < 0.8f) return ShortPlatform;
            return SpikesPlatform;
        }

        // 150Ц200: обычные исчезают, по€вл€ютс€ короткие с шипами
        else if (height < 200f)
        {
            if (r < 0.5f) return ShortPlatform;
            if (r < 0.8f) return SpikesPlatform;
            return ShortSpikePlatform;
        }

        // 200Ц250: исчезающие
        else if (height < 250f)
        {
            if (r < 0.6f) return DisappearingPlatform;
            if (r < 0.85f) return SpikesPlatform;
            return FlyingPlatform;
        }

        // 250Ц300: исчезающие + шипы
        else if (height < 300f)
        {
            if (r < 0.20f) return SpikesPlatform;
            if (r < 0.5f) return ShortDisapearPlatform;
            if (r < 0.75f) return ShortPlatform;
            return FlyingPlatform;
        }

        // 300Ц350: короткие исчезающие
        else if (height < 350f)
        {
            if (r < 0.6f) return ShortDisapearPlatform;
            if (r < 0.85f) return DisappearingPlatform;
            return FlyingPlatform;
        }

        // 350Ц400: короткие исчезающие + короткие шипы
        else if (height < 400f)
        {
            if (r < 0.2f) return ShortSpikePlatform;
            if (r < 0.5f) return ShortDisapearPlatform;
            return FlyingPlatform;
        }

        // 400Ц450: вертикальное мышление
        else if (height < 450f)
        {
            if (r < 0.1f) return ShortSpikePlatform;
            if (r < 0.3f) return VerticalPlatform;
            if (r < 0.8f) return NormalPlatform;
            return FlyingPlatform;
        }

        // 450Ц500: длинные вертикали + наказание
        else if (height < 500f)
        {
            if (r < 0.25f) return SpikesPlatform;
            if (r < 0.65f) return ShortDisapearPlatform;
            else return ShortPlatform;
        }

        
        else if (height < 600f)
        {
            if (r < 0.5f) return VerticalPlatform;
            if (r < 0.65f) return ShortDisapearPlatform;
            if (r < 0.75f) return DisappearingPlatform;
            return FlyingPlatform;
        }

        
        else if (height < 700f)
        {
            if (r < 0.45f) return ShortDisapearPlatform;
            if (r < 0.7f) return SpikesPlatform;
            if (r < 0.85f) return NormalPlatform;
            return FlyingPlatform;
        }

        // поздн€€ фаза Ч всЄ, кроме хал€вы
        else
        {
            if (r < 0.15f) return ShortDisapearPlatform;
            if (r < 0.35f) return FlyingPlatform;
            if (r < 0.5f) return VerticalPlatform;
            if (r < 0.6f) return NormalPlatform;
            if (r < 0.7f) return DisappearingPlatform;
            if (r < 0.8f) return ShortPlatform;
            if (r < 0.9f) return ShortSpikePlatform;
            return SpikesPlatform;
        }
    }

    private void CleanupPlatforms()
    {
        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            var platform = _activePlatforms[i];
            var falling = platform.GetComponent<FallingPlatform>();

            if (platform.transform.position.y <= LavaTransform.position.y)
            {
                // перед возвратом сбрасываем состо€ние
                falling?.ResetPlatform();

                PoolManager.Instance.ReturnObject(platform);
                MonsterSpawner?.RemovePlatform(platform);
                _activePlatforms.RemoveAt(i);
            }
        }
    }

    public List<GameObject> GetActivePlatforms() => _activePlatforms;

    public void ForceSpawnSafePlatformAt(float height) 
    {
        if (NormalPlatform == null)
        {
            return;
        }

        // выбираем X так же, как у обычных платформ
        float xPos = ChooseXSlot(NormalPlatform);

        Vector3 spawnPos = new Vector3(xPos, height, 0f);

        GameObject platformObj = PoolManager.Instance.GetObject(
            NormalPlatform,
            spawnPos,
            NormalPlatform.transform.rotation
        );

        if (platformObj == null)
            return;

        _activePlatforms.Add(platformObj);

        // если вдруг на префабе есть FallingPlatform Ч принудительно сбрасываем
        if (platformObj.TryGetComponent<FallingPlatform>(out var falling))
        {
            falling.ResetPlatform();
        }

    }

}
