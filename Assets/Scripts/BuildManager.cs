using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    [HideInInspector]
    public bool isRemoveToolSelected;
    [HideInInspector]
    public bool isUpgrading;
    
    private MoneyManager moneyManager;
    private PlantBlueprint toBuild;

    private void Start()
    {
        isRemoveToolSelected = false;
        isUpgrading = false;
        moneyManager = MoneyManager.Instance;
    }

    public bool canBuild() => toBuild != null;

    public bool hasMoney() => moneyManager.money >= toBuild.cost;

    public void selectPlantToBuild(PlantBlueprint plantBlueprint)
    {
        toBuild = plantBlueprint;
        isRemoveToolSelected = false;
        isUpgrading = plantBlueprint == null ? false : plantBlueprint.isUpgradePlant;
        AudioManager.Instance.play("Select");
    }

    public void buildPlantOn(Node node)
    {
        if (moneyManager.money < toBuild.cost)
        {
            selectPlantToBuild(null);
            Debug.Log("Not enough money");
            return;
        }

        moneyManager.useMoney(toBuild.cost);
        var plant = Instantiate(toBuild.prefab, node.transform.position, node.transform.rotation);
        node.plant = plant;
        node.isPlantUpgradeable = toBuild.isUpgradeable;
        selectPlantToBuild(null);
        AudioManager.Instance.play("Plant");
    }

    public void upgradePlantOn(Node node)
    {
        // TODO : remove duplicate code
        if (moneyManager.money < toBuild.cost)
        {
            selectPlantToBuild(null);
            Debug.Log("Not enough money");
            return;
        }

        if (!node.isPlantUpgradeable)
        {
            selectPlantToBuild(null);
            Debug.Log("Not upgradeable plant");
            return;
        }

        moneyManager.useMoney(toBuild.cost);
        var plant = Instantiate(toBuild.prefab, node.transform.position, node.transform.rotation);
        Destroy(node.plant);
        node.plant = plant;
        node.isPlantUpgradeable = toBuild.isUpgradeable;
        selectPlantToBuild(null);
        AudioManager.Instance.play("Plant");
    }

    public void removeToolClicked()
    {
        isRemoveToolSelected = !isRemoveToolSelected;
        if (isRemoveToolSelected)
        {
            toBuild = null;
            isUpgrading = false;
        }

        AudioManager.Instance.play("Select");
    }
}