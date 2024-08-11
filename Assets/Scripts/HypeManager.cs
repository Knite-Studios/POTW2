using System.Collections.Generic;
using MiniGame;
using UnityEngine;

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

    private readonly Queue<Hype> _hypes = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnHype();
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

    public void TriggerHype()
    {
        if (_hypes.Count == 0 || !hypeTrigger) return;
        
        var hype = _hypes.Dequeue();
        var distance = Vector3.Distance(hype.transform.position, hypeTrigger.transform.position);
        Debug.Log($"Distance: {distance}");

        int reward;
        // TODO: Improve this lol.
        if (distance <= perfectHypeTime)
        {
            reward = (int) (baseReward * perfectMultiplier);
            Debug.Log("PERFECT! 2x multiplier!");
        }
        else if (distance <= niceHypeTime)
        {
            reward = (int) (baseReward * niceMultiplier);
            Debug.Log("NICE CATCH! 1.5x multiplier!");
        }
        else if (distance <= prematureHypeTime)
        {
            reward = (int) (baseReward * prematureMultiplier);
            Debug.Log("PREMATURE! 1.0x multiplier!");
        }
        else
        {
            reward = 0;
            Debug.Log("Missed Hype!");
        }
        
        MoneyManager.Instance.gainMoney(reward);
        
        Destroy(hype.gameObject);
        // Destroy(_hypeTrigger.gameObject);
    }
}