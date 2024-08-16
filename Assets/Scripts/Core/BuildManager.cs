using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public bool IsRemoveToolSelected { get; private set; }
    public bool IsUpgrading { get; private set; }
    
    private MoneyManager moneyManager;
    private PlantBlueprint plantToBuild;

    private void Start()
    {
        IsRemoveToolSelected = false;
        IsUpgrading = false;
        moneyManager = MoneyManager.Instance;
    }

    public bool CanBuild() => plantToBuild != null;

    public bool HasMoney() => plantToBuild != null && moneyManager.Money >= plantToBuild.cost;

    public void SelectPlantToBuild(PlantBlueprint plantBlueprint)
    {
        plantToBuild = plantBlueprint;
        IsRemoveToolSelected = false;
        IsUpgrading = plantBlueprint != null && plantBlueprint.isUpgradePlant;
        AudioManager.Instance.Play("Select");
        Debug.Log($"Selected plant: {(plantToBuild != null ? plantToBuild.prefab.name : "None")}");
    }

    public void BuildPlantOn(Node node)
    {
        if (!CanBuildPlant())
        {
            Debug.Log("Cannot build plant: " + (plantToBuild == null ? "No plant selected" : "Not enough money"));
            return;
        }

        moneyManager.UseMoney(plantToBuild.cost);
        InstantiatePlant(node);
        AudioManager.Instance.Play("Plant");
    }

    public void UpgradePlantOn(Node node)
    {
        if (!CanUpgradePlant(node))
        {
            return;
        }

        moneyManager.UseMoney(plantToBuild.cost);
        Object.Destroy(node.plant);
        InstantiatePlant(node);
        AudioManager.Instance.Play("Plant");
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
    }
}