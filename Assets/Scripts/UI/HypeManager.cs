using UnityEngine;
using MiniGame;
using System.Collections.Generic;

public class HypeManager : Singleton<HypeManager>
{
    [Header("Reward Settings")]
    public int baseReward = 25;
    public float caughtMultiplier = 1.0f;
    
    [Header("Hype Settings")]
    public GameObject hypePrefab;
    public HypeTrigger hypeTrigger;
    public Transform hypeSpawnPoint;
    public float caughtDistance = 10.0f;
    public GameObject caughtPrefab;
    public GameObject missedPrefab;

    private readonly Queue<Hype> _hypes = new Queue<Hype>();
    [field: SerializeField] public int HypeCount { get; private set; }

    private void Update()
    {
        HypeCount = _hypes.Count;
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnHype();
        }
#endif
        
        if (InputManager.Hype.triggered)
        {
            var hype = _hypes.Peek();
            TriggerHype(hype);
        }
    }

    public void SpawnHype()
    {
        var hypeObject = Instantiate(hypePrefab, hypeSpawnPoint.parent);
        var hype = hypeObject.GetComponent<Hype>();
        hype.Initialize(hypeTrigger);
        hype.OnHypeDestroyed += () =>
        {
            _hypes.TryPeek(out var curHype);
            if (curHype && curHype.Target == hypeTrigger.transform && hype == curHype)
                _hypes.Dequeue();
        };
        
        _hypes.Enqueue(hype);
    }

    public void TriggerHype(Hype hype)
    {
        if (_hypes.Count == 0 || !hypeTrigger) return;

        var distance = Vector3.Distance(hype.transform.position, hypeTrigger.transform.position);
        Debug.Log($"Distance: {distance}");

        int reward;
        GameObject feedback;
        if (distance <= caughtDistance)
        {
            reward = (int)(baseReward * caughtMultiplier);
            feedback = caughtPrefab;
            Debug.Log("Caught! 1x multiplier!");
        }
        else
        {
            reward = 0;
            feedback = missedPrefab;
            Debug.Log("Missed Hype!");
        }
        
        MoneyManager.Instance.CollectHype(reward);
        
        if (feedback)
        {
            var feedbackInstance = Instantiate(feedback, hypeTrigger.transform.position, Quaternion.identity, hypeSpawnPoint.parent);
            feedbackInstance.GetComponent<Rigidbody2D>().velocity = Vector2.up * 100.0f;
            Destroy(feedbackInstance, 1.0f);
        }
        
        Destroy(hype.gameObject);
    }
}