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
        var index = (int)(transform.position.x / 4);
        target = WayPoints.points[index];

        isEatingHash = Animator.StringToHash("isEating");
    }

    private void Update()
    {
        if (!isEating)
        {
            var dir = target.position - transform.position;
            transform.Translate(dir.normalized * zombie.speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
                reachedEnd();
            }

            if (zombie.slownessTimer <= 0f)
            {
                zombie.speed = zombie.defaultSpeed;
            }
            else
            {
                zombie.slownessTimer -= Time.deltaTime;
            }
        }
        else if (plant == null)
        {
            isEating = false;
            animator.SetBool(isEatingHash, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Plant")
        {
            isEating = true;
            plant = other;
            animator.SetBool(isEatingHash, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Plant")
        {
            isEating = false;
            animator.SetBool(isEatingHash, false);
        }
    }

    private void reachedEnd()
    {
        WaveSpawner.zombiesAlive--;
        Destroy(gameObject);
    }
}