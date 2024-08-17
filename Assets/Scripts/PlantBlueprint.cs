using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlantBlueprint
{
    public GameObject prefab;
    public int cost;
    public Text costText;
    public bool isUpgradePlant;
    public bool isUpgradeable;
    public float cooldown;
    
    [HideInInspector] public bool isOnCooldown;
}