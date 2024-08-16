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

        Debug.Log($"Attempting to purchase plant: {item.plantBlueprint.prefab.name}");
        
        BuildManager.Instance.SelectPlantToBuild(item.plantBlueprint);
        Debug.Log($"Selected plant for building: {item.plantBlueprint.prefab.name}");
    }
}