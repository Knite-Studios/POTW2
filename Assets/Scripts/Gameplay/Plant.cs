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
        Debug.Log($"Plant initialized. Fire range: {fireRange}, Zombie tag: {zombieTag}");
    }

    private void Update()
    {
        if (!target)
        {
            return;
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            UpdateFireCountdown();
        }

        fireCountdown -= Time.deltaTime;
    }

    private void UpdateFireCountdown()
    {
        if (!hasDoubleShoot)
        {
            fireCountdown = firstFireTime;
        }
        else
        {
            fireCountdown = isCurrentFirst ? firstFireTime : secondFireTime;
            isCurrentFirst = !isCurrentFirst;
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(zombieTag);
        Debug.Log($"Found {enemies.Length} enemies with tag '{zombieTag}'");

        float shortestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (IsEnemyInRange(enemy, out float distanceToEnemy))
            {
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    closestEnemy = enemy;
                }
            }
        }

        target = closestEnemy != null ? closestEnemy.transform : null;
        Debug.Log(target != null ? $"New target acquired at distance {shortestDistance}" : "No target in range");
    }

    private bool IsEnemyInRange(GameObject enemy, out float distanceToEnemy)
    {
        distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
        bool inRange = Mathf.Abs(enemy.transform.position.x - transform.position.x) < 1 &&
                       enemy.transform.position.z > transform.position.z &&
                       distanceToEnemy < fireRange;
        
        if (inRange)
        {
            Debug.Log($"Enemy in range at distance {distanceToEnemy}");
        }
        
        return inRange;
    }

    private void Shoot()
    {
        Vector3 bulletPosition = transform.position + Vector3.up * 2f;
        GameObject bulletGO = Instantiate(bulletPrefab, bulletPosition, transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.Seek(target);
        Debug.Log($"Bullet shot towards target at {target.position}");
    }
}