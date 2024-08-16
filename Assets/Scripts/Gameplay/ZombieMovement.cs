using UnityEngine;

[RequireComponent(typeof(Zombie))]
public class ZombieMovement : MonoBehaviour
{
    public Animator animator;
    public Collider plant;
    
    private bool isEating;
    private int isEatingHash;
    private Transform target;
    private int waveNumber;
    private Zombie zombie;
    
    private void Start()
    {
        zombie = GetComponent<Zombie>();
        int index = (int)(transform.position.x / 4);
        target = WayPoints.points[index];

        isEatingHash = Animator.StringToHash("isEating");
    }

    private void Update()
    {
        if (!isEating)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * (zombie.Speed * Time.deltaTime), Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
                ReachedEnd();
            }

            if (zombie.SlownessTimer <= 0f)
            {
                zombie.Speed = zombie.DefaultSpeed;
            }
            else
            {
                zombie.SlownessTimer -= Time.deltaTime;
            }
        }
        else if (!plant)
        {
            isEating = false;
            animator.SetBool(isEatingHash, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Plant"))
        {
            isEating = true;
            plant = other;
            animator.SetBool(isEatingHash, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Plant"))
        {
            isEating = false;
            animator.SetBool(isEatingHash, false);
        }
    }

    private void ReachedEnd()
    {
        WaveSpawner.zombiesAlive--;
        Destroy(gameObject);
    }
}