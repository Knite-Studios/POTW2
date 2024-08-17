using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlantBlueprint
{
    public GameObject prefab;
    public int cost;
    public bool isUpgradePlant;
    public bool isUpgradeable;
    public float cooldown;
    public Image cooldownImage;

    [HideInInspector] public bool isOnCooldown;
}