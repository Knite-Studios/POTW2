using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool canSlowZombie;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
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
    }

    private void HitTarget()
    {
        if (target != null)
        {
            MakeDamage(target);
        }

        AudioManager.Instance.Play("HitNormal");
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
            }
            else
            {
                zombie.TakeDamage(damage);
            }
        }
    }
}