using System.Collections;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public bool IsRemoveToolSelected { get; private set; }
    public bool IsUpgrading { get; private set; }
    
    private MoneyManager moneyManager;
    private PlantBlueprint plantToBuild;
    private Shop shop; // Reference to the Shop script

    private void Start()
    {
        IsRemoveToolSelected = false;
        IsUpgrading = false;
        moneyManager = MoneyManager.Instance;
        shop = FindObjectOfType<Shop>(); // Find the Shop script in the scene
    }

    public bool CanBuild() => plantToBuild != null;

    public bool HasMoney() => plantToBuild != null && moneyManager.Money >= plantToBuild.cost;

    public void SelectPlantToBuild(PlantBlueprint plantBlueprint)
    {
        if (plantBlueprint.isOnCooldown)
        {
            Debug.Log("Plant is on cooldown");
            return;
        }
        
        plantToBuild = plantBlueprint;
        IsRemoveToolSelected = false;
        IsUpgrading = plantBlueprint != null && plantBlueprint.isUpgradePlant;
        AudioManager.Instance.Play("Select");
        Debug.Log($"Selected plant: {(plantToBuild != null ? plantToBuild.prefab.name : "None")}");
        shop.ShowInstructionsUI(true);
    }

    public void DeselectPlant()
    {
        plantToBuild = null;
        IsRemoveToolSelected = false;
        IsUpgrading = false;
        Debug.Log("Plant deselected");
        shop.ShowInstructionsUI(false);
    }

    public void BuildPlantOn(Node node)
    {
        if (!CanBuildPlant())
        {
            Debug.Log("Cannot build plant: " + (plantToBuild == null ? "No plant selected" : "Not enough money"));
            return;
        }
        
        if (plantToBuild.isOnCooldown)
        {
            SelectPlantToBuild(null);
            Debug.Log("Plant is on cooldown");
            return;
        }

        StartCoroutine(StartCooldown(plantToBuild));
        moneyManager.UseMoney(plantToBuild.cost);
        InstantiatePlant(node);
        AudioManager.Instance.Play("Plant");
        shop.OnPlantPlaced();
    }

    public void UpgradePlantOn(Node node)
    {
        if (!CanUpgradePlant(node))
        {
            return;
        }
        
        if (plantToBuild.isOnCooldown)
        {
            SelectPlantToBuild(null);
            Debug.Log("Plant is on cooldown");
            return;
        }
        
        StartCoroutine(StartCooldown(plantToBuild));
        moneyManager.UseMoney(plantToBuild.cost);
        Object.Destroy(node.plant);
        InstantiatePlant(node);
        AudioManager.Instance.Play("Plant");
        shop.OnPlantPlaced();
    }

    public void ToggleRemoveTool()
    {
        IsRemoveToolSelected = !IsRemoveToolSelected;
        if (IsRemoveToolSelected)
        {
            plantToBuild = null;
            IsUpgrading = false;
        }

        AudioManager.Instance.Play("Select");
        Debug.Log($"Remove tool {(IsRemoveToolSelected ? "selected" : "deselected")}");
        shop.ShowInstructionsUI(false);
    }

    private bool CanBuildPlant()
    {
        if (plantToBuild == null)
        {
            Debug.Log("No plant selected to build");
            return false;
        }
        if (moneyManager.Money < plantToBuild.cost)
        {
            Debug.Log("Not enough money to build");
            return false;
        }
        return true;
    }

    private bool CanUpgradePlant(Node node)
    {
        if (!CanBuildPlant())
        {
            return false;
        }

        if (!node.isPlantUpgradeable)
        {
            Debug.Log("Not upgradeable plant");
            return false;
        }
        return true;
    }

    private void InstantiatePlant(Node node)
    {
        GameObject plant = Instantiate(plantToBuild.prefab, node.transform.position, node.transform.rotation);
        node.plant = plant;
        node.isPlantUpgradeable = plantToBuild.isUpgradeable;
        Debug.Log($"Plant {plantToBuild.prefab.name} instantiated");
        // Don't clear plantToBuild here to allow building multiple plants of the same type
    }

    public void ClearSelectedPlant()
    {
        plantToBuild = null;
        Debug.Log("Selected plant cleared");
        shop.ShowInstructionsUI(false);
    }
    
    private IEnumerator StartCooldown(PlantBlueprint plantBlueprint)
    {
        plantBlueprint.isOnCooldown = true;

        if (plantBlueprint.cooldownImage)
            plantBlueprint.cooldownImage.fillAmount = 1;
        
        var cooldown = plantBlueprint.cooldown;
        var time = 0f;
        
        while (time < cooldown)
        {
            time += Time.deltaTime;
            var fillAmount = Mathf.Lerp(1, 0, time / cooldown);
            if (plantBlueprint.cooldownImage)
                plantBlueprint.cooldownImage.fillAmount = fillAmount;
            yield return null;
        }

        plantBlueprint.isOnCooldown = false;
        if (plantBlueprint.cooldownImage)
            plantBlueprint.cooldownImage.fillAmount = 0;
    }
}