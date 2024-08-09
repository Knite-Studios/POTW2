using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("Attributes")]
    public float fireRange = 20f;
    public float firstFireTime = 2f;
    public float secondFireTime;
    public string zombieTag = "Zombie";
    public GameObject bulletPrefab;
    
    private float fireCountdown;
    private bool hasDoubleShoot;
    private bool isCurrentFirst = true;
    private Transform target;

    private void Start()
    {
        hasDoubleShoot = secondFireTime != 0f;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    private void Update()
    {
        if (!target)
        {
            return;
        }

        if (fireCountdown <= 0f)
        {
            shoot();
            if (!hasDoubleShoot)
            {
                fireCountdown = firstFireTime;
            }
            else
            {
                // TODO: fix the delay that happens sometime
                if (isCurrentFirst)
                {
                    fireCountdown = firstFireTime;
                }
                else
                {
                    fireCountdown = secondFireTime;
                }

                isCurrentFirst = !isCurrentFirst;
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    private void UpdateTarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag(zombieTag);
        var shortestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;
        foreach (var enemy in enemies)
        {
            var distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (Mathf.Abs(enemy.transform.position.x - transform.position.x) < 1)
            {
                if (enemy.transform.position.z > transform.position.z)
                {
                    if (distanceToEnemy < fireRange)
                    {
                        if (distanceToEnemy < shortestDistance)
                        {
                            shortestDistance = distanceToEnemy;
                            closestEnemy = enemy;
                        }
                    }
                }
            }

        }

        if (closestEnemy != null)
        {
            // TODO : check they are on the same lane too
            target = closestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void shoot()
    {
        var newPosition = transform.position;
        newPosition.y += 2f;
        var bulletGO = Instantiate(bulletPrefab, newPosition, transform.rotation);
        var bullet = bulletGO.GetComponent<Bullet>();
        bullet.seek(target);
    }
}