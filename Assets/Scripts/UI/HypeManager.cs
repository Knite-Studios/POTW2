using UnityEngine;
using System.Collections.Generic;
using MiniGame;

public class HypeManager : Singleton<HypeManager>
{
    [Header("Reward Settings")]
    public int baseReward = 25;
    public float perfectMultiplier = 2.0f;
    public float niceMultiplier = 1.5f;
    public float prematureMultiplier = 1.0f;
    
    [Header("Hype Settings")]
    public GameObject hypePrefab;
    public HypeTrigger hypeTrigger;
    public Transform hypeSpawnPoint;
    public float perfectHypeTime = 2.0f;
    public float niceHypeTime = 5.0f;
    public float prematureHypeTime = 10.0f;

    private Queue<Hype> _hypes = new Queue<Hype>();

    private void Update()
    {
        if (InputManager.Hype.triggered)
        {
            TriggerHype();
        }
    }

    public void SpawnHype()
    {
        if (hypeSpawnPoint == null || hypePrefab == null || hypeTrigger == null)
        {
            Debug.LogError("HypeManager: Missing references for spawning hype.");
            return;
        }

        var hypeObject = Instantiate(hypePrefab, hypeSpawnPoint.position, Quaternion.identity, hypeSpawnPoint);
        var hype = hypeObject.GetComponent<Hype>();
        if (hype != null)
        {
            hype.Initialize(hypeTrigger);
            hype.OnHypeDestroyed += () => RemoveHypeFromQueue(hype);
            _hypes.Enqueue(hype);
        }
        else
        {
            Debug.LogError("HypeManager: Spawned object does not have a Hype component.");
        }
    }

    private void RemoveHypeFromQueue(Hype hype)
    {
        if (_hypes.Count > 0 && _hypes.Peek() == hype)
        {
            _hypes.Dequeue();
        }
    }

    public void TriggerHype()
    {
        if (_hypes.Count == 0 || hypeTrigger == null) return;
        
        var hype = _hypes.Dequeue();
        if (hype == null) return;

        var distance = Vector3.Distance(hype.transform.position, hypeTrigger.transform.position);
        Debug.Log($"Distance: {distance}");

        int reward;
        if (distance <= perfectHypeTime)
        {
            reward = (int)(baseReward * perfectMultiplier);
            Debug.Log("PERFECT! 2x multiplier!");
        }
        else if (distance <= niceHypeTime)
        {
            reward = (int)(baseReward * niceMultiplier);
            Debug.Log("NICE CATCH! 1.5x multiplier!");
        }
        else if (distance <= prematureHypeTime)
        {
            reward = (int)(baseReward * prematureMultiplier);
            Debug.Log("PREMATURE! 1.0x multiplier!");
        }
        else
        {
            reward = 0;
            Debug.Log("Missed Hype!");
        }
        
        MoneyManager.Instance.CollectHype(reward);
        
        Destroy(hype.gameObject);
    }

    public void ResetHype()
    {
        foreach (var hype in _hypes)
        {
            if (hype != null)
            {
                Destroy(hype.gameObject);
            }
        }
        _hypes.Clear();
    }
}