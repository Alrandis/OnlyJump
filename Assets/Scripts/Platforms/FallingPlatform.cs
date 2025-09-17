using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float delayBeforeDisappear = 1f;
    [SerializeField] private float fallSpeed = 5f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private bool triggered = false;
    private float disappearTimer = 0f;

    public bool MarkedForRemoval { get; private set; } = false;

    public void UpdateDisappearDelay(float playerY)
    {
        // при создании можно обновить таймер (если надо)
        disappearTimer = delayBeforeDisappear;
        triggered = false;
        MarkedForRemoval = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!triggered && collision.collider.CompareTag("Player"))
        {
            triggered = true;
            disappearTimer = delayBeforeDisappear;
        }
    }

    private void Update()
    {
        if (triggered && !MarkedForRemoval)
        {
            animator.SetTrigger("Disappear");
            disappearTimer -= Time.deltaTime;
            if (disappearTimer <= 0f)
            {
                MarkedForRemoval = true;
                triggered = false;
               
                GetComponent<Collider2D>().enabled = false;
                GetComponentInChildren<SpriteRenderer>().enabled = false; 
            }
        }
    }

    public void ResetPlatform()
    {
        triggered = false;
        MarkedForRemoval = false;
        disappearTimer = 0f;
        // включаем обратно компоненты
        GetComponent<Collider2D>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        animator.SetTrigger("Idle");
    }
}
