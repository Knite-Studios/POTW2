using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 5;
    public float speed = 1f;
    public bool canSlowZombie;

    private Transform target;

    private void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        var dir = target.position - transform.position;
        var distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void seek(Transform _target)
    {
        target = _target;
    }

    private void HitTarget()
    {
        Destroy(gameObject);
        makeDamage(target);
        // TODO: change sound depending on zombie type
        AudioManager.Instance.play("HitNormal");
    }

    private void makeDamage(Transform target)
    {
        if (target != null)
        {
            var zombie = target.GetComponent<Zombie>();
            if (canSlowZombie)
            {
                zombie.slowAndDamge(damage, 2f);
            }
            else
            {
                zombie.takeDamage(damage);
            }
        }
    }
}