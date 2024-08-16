using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float DefaultSpeed = 5f;
    public int DefaultHealth = 20;
    
    public int Health { get; private set; }
    public float Speed { get; set; }
    public float SlownessTimer { get; set; }
    
    private bool isDead;

    private void Start()
    {
        isDead = false;
        Health = DefaultHealth;
        Speed = DefaultSpeed;
        SlownessTimer = 0f;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        WaveSpawner.zombiesAlive--;
        Destroy(gameObject);
    }

    public void SlowAndDamage(int amount, float slownessTime)
    {
        TakeDamage(amount);
        if (Health > 0)
        {
            Speed = DefaultSpeed / 2;
            SlownessTimer = slownessTime;
        }
    }
}