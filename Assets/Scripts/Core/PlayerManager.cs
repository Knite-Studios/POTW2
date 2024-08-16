using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [HideInInspector]
    public int rounds;
    private void Start()
    {
        rounds = 0;
    }
}