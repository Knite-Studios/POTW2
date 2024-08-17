using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    /// <summary>
    /// UI element that spawns the hype object.
    /// </summary>
    public class Hype : MonoBehaviour
    {
        public float missOffset = 20.0f;
        public UnityAction OnHypeDestroyed;

        [SerializeField] private float speed = 200.0f;

        public Transform Target { get; private set; }
        public bool CanTrigger { get; private set; }

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            if (!(transform.position.x < Target.position.x - missOffset)) return;
            
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.GetComponent<HypeTrigger>()) return;
            
            CanTrigger = true;
        }
    }
}