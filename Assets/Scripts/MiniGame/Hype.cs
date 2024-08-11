using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    /// <summary>
    /// UI element that spawns the hype object.
    /// </summary>
    public class Hype : MonoBehaviour
    {
        public UnityAction OnHypeDestroyed;

        [SerializeField] private float speed = 200.0f;

        public Transform Target { get; private set; }

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            // _rb.gravityScale = 0.0f;
        }
        
        private void Update()
        {
            if (!(transform.position.x < Target.position.x - 20.0f)) return;
            
            Debug.Log("<color=red>Missed the hype without pressing!</color>");
            Destroy(gameObject);
            // Destroy(Target!.gameObject);
        }

        private void OnDestroy()
        {
            // Invoke the event first.
            OnHypeDestroyed?.Invoke();
            
            // Unsubscribe to all handlers.
            foreach (var d in OnHypeDestroyed!.GetInvocationList())
            {
                OnHypeDestroyed -= (UnityAction) d;
            }
        }

        public void Initialize(HypeTrigger owner)
        {
            Target = owner.transform;
            _rb.velocity = Vector2.left * speed;
        }
    }
}