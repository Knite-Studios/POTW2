using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool canSlowZombie;

    private Transform target;

    private void Start()
    {
        Debug.Log($"Bullet initialized. Damage: {damage}, Speed: {speed}, Can slow zombie: {canSlowZombie}");
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("Bullet target lost. Destroying bullet.");
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void Seek(Transform newTarget)
    {
        target = newTarget;
        Debug.Log($"Bullet seeking new target at {target.position}");
    }

    private void HitTarget()
    {
        if (target != null)
        {
            MakeDamage(target);
        }

        AudioManager.Instance.Play("HitNormal");
        Debug.Log("Bullet hit target. Destroying bullet.");
        Destroy(gameObject);
    }

    private void MakeDamage(Transform targetTransform)
    {
        Zombie zombie = targetTransform.GetComponent<Zombie>();
        if (zombie != null)
        {
            if (canSlowZombie)
            {
                zombie.SlowAndDamage(damage, 2f);
                Debug.Log($"Applied slow and {damage} damage to zombie");
            }
            else
            {
                zombie.TakeDamage(damage);
                Debug.Log($"Applied {damage} damage to zombie");
            }
        }
        else
        {
            Debug.LogWarning("Target is not a zombie!");
        }
    }
}