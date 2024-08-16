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

    public bool HasMoney() => moneyManager.Money >= plantToBuild.cost;

    public void SelectPlantToBuild(PlantBlueprint plantBlueprint)
    {
        plantToBuild = plantBlueprint;
        IsRemoveToolSelected = false;
        IsUpgrading = plantBlueprint != null && plantBlueprint.isUpgradePlant;
        AudioManager.Instance.Play("Select");
    }

    public void BuildPlantOn(Node node)
    {
        if (!CanBuildPlant())
        {
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
    }

    private bool CanBuildPlant()
    {
        if (moneyManager.Money < plantToBuild.cost)
        {
            SelectPlantToBuild(null);
            Debug.Log("Not enough money");
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
            SelectPlantToBuild(null);
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
        SelectPlantToBuild(null);
    }
}