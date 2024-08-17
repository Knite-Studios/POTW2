using UnityEngine;

public class Shop : MonoBehaviour
{
    public PlantBlueprint plant1;
    public PlantBlueprint plant2;
    public PlantBlueprint plant3;
    public PlantBlueprint plant4;

    private void Start()
    {
        plant1.costText.text = plant1.cost + "$";
        plant2.costText.text = plant2.cost + "$";
        if (plant3.costText != null)
        {
            plant3.costText.text = plant3.cost + "$";
        }

        if (plant4.costText != null)
        {
            plant4.costText.text = plant4.cost + "$";
        }
    }

    public void purchasePlant1()
    {
        if (plant1.isOnCooldown)
        {
            Debug.Log("Plant 1 is on cooldown");
            return;
        }
        
        Debug.Log("purchased plant 1");
        BuildManager.Instance.selectPlantToBuild(plant1);
    }

    public void purchasePlant2()
    {
        if (plant2.isOnCooldown)
        {
            Debug.Log("Plant 2 is on cooldown");
            return;
        }
        
        Debug.Log("purchased plant 2");
        BuildManager.Instance.selectPlantToBuild(plant2);
    }

    public void purchasePlant3()
    {
        if (plant3.isOnCooldown)
        {
            Debug.Log("Plant 3 is on cooldown");
            return;
        }
        
        Debug.Log("purchased plant 3");
        BuildManager.Instance.selectPlantToBuild(plant3);
    }

    public void purchasePlant4()
    {
        if (plant4.isOnCooldown)
        {
            Debug.Log("Plant 4 is on cooldown");
            return;
        }
        
        Debug.Log("purchased plant 4");
        BuildManager.Instance.selectPlantToBuild(plant4);
    }
}