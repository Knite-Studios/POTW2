using UnityEngine;
using UnityEngine.Events;

public class HealthObject : MonoBehaviour
{
    public float health = 5f;
    public UnityEvent objectDiedCallback;
    
    private float currentHealth;

    private void Start()
    {
        currentHealth = health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            // Debug.Log("Zombie approached an object");
        }
        else if (other.gameObject.CompareTag("ZombieArm"))
        {
            currentHealth -= 1f;
            checkDeath();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            Debug.Log("Zombie disappeared");
        }
    }

    private void checkDeath()
    {
        if (currentHealth == 0)
        {
            Destroy(gameObject);
            if (objectDiedCallback != null)
            {
                objectDiedCallback.Invoke();
            }
        }
    }
}