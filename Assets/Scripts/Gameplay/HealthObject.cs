using UnityEngine;
using UnityEngine.Events;

public class HealthObject : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private bool isBrain = false;
    
    public UnityEvent objectDiedCallback;
    
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieArm"))
        {
            TakeDamage(1f);
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (currentHealth <= 0)
        {
            if (isBrain)
            {
                GameManager.Instance.EndGame();
            }
            else
            {
                Destroy(gameObject);
                objectDiedCallback?.Invoke();
            }
        }
    }
}