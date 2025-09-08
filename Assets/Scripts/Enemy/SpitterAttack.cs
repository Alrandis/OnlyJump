using UnityEngine;

public class SpitterAttack : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform FirePoint;
    public float FireRate = 2f;
    public float ProjectileSpeed = 5f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= FireRate)
        {
            Shoot();
            _timer = 0;
        }
    }

    private void Shoot()
    {
        GameObject proj = Instantiate(ProjectilePrefab, FirePoint.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * ProjectileSpeed; // стреляет влево (можно сделать гибко)
    }
}
