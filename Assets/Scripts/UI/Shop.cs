using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public PlantBlueprint plantBlueprint;
        public Text costText;
    }

    public List<ShopItem> shopItems = new List<ShopItem>();
    
    [SerializeField] private GameObject instructionsUI;

    private void Start()
    {
        UpdateCostTexts();
    }

    private void UpdateCostTexts()
    {
        foreach (var item in shopItems)
        {
            if (item.costText != null)
            {
                item.costText.text = $"{item.plantBlueprint.cost}$";
            }
            else
            {
                Debug.LogWarning($"Cost text is missing for plant: {item.plantBlueprint.prefab.name}");
            }
        }
    }

    public void PurchasePlant(int index)
    {
        if (index < 0 || index >= shopItems.Count)
        {
            Debug.LogError($"Invalid plant index: {index}");
            return;
        }

        var item = shopItems[index];
        if (item.plantBlueprint == null)
        {
            Debug.LogError($"Plant blueprint is missing for index: {index}");
            return;
        }

        if (item.plantBlueprint.isOnCooldown)
        {
            Debug.Log("Plant is on cooldown");
            return;
        }

        Debug.Log($"Attempting to purchase plant: {item.plantBlueprint.prefab.name}");

        BuildManager.Instance.SelectPlantToBuild(item.plantBlueprint);
        Debug.Log($"Selected plant for building: {item.plantBlueprint.prefab.name}");
    }

    public void ShowInstructionsUI(bool show)
    {
        if (instructionsUI != null)
        {
            instructionsUI.SetActive(show);
        }
        else
        {
            Debug.LogWarning("Instructions UI is not assigned in the Shop script");
        }
    }

    public void OnPlantPlaced()
    {
        ShowInstructionsUI(false);
    }
}