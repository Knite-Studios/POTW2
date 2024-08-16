using UnityEngine;
using System.Collections.Generic;

public class FreezeTower : MonoBehaviour
{
    [Header("Attributes")]
    public float freezeRange = 10f;
    public float freezeDuration = 2f;
    public float freezeCooldown = 3f;
    public string zombieTag = "Zombie";

    private float cooldownTimer;
    private List<Zombie> zombiesInRange = new List<Zombie>();

    private void Update()
    {
        UpdateZombiesInRange();

        if (cooldownTimer <= 0f && zombiesInRange.Count > 0)
        {
            FreezeZombies();
            cooldownTimer = freezeCooldown;
        }

        cooldownTimer -= Time.deltaTime;
    }

    private void UpdateZombiesInRange()
    {
        zombiesInRange.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, freezeRange);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(zombieTag))
            {
                Zombie zombie = collider.GetComponent<Zombie>();
                if (zombie != null)
                {
                    zombiesInRange.Add(zombie);
                }
            }
        }
    }

    private void FreezeZombies()
    {
        foreach (Zombie zombie in zombiesInRange)
        {
            zombie.SlowAndDamage(0, freezeDuration); // 0 damage, only slowing
        }
        // Play freeze effect
        AudioManager.Instance.Play("Freeze");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, freezeRange);
    }
}