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
        Debug.Log($"Purchased plant {index + 1}");
        BuildManager.Instance.SelectPlantToBuild(item.plantBlueprint);
    }
}