using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color errorColor;
    public GameObject plant;
    public bool isPlantUpgradeable;
    
    private Renderer rend;
    private Color startColor;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        if (BuildManager.Instance.IsRemoveToolSelected)
        {
            HandleRemoveTool();
            return;
        }

        if (!BuildManager.Instance.CanBuild())
        {
            Debug.Log("No plant selected to build");
            return;
        }

        if (BuildManager.Instance.IsUpgrading)
        {
            HandleUpgrade();
        }
        else
        {
            HandleBuild();
        }

        rend.material.color = startColor;
    }

    private void HandleRemoveTool()
    {
        if (plant != null)
        {
            Destroy(plant);
            isPlantUpgradeable = false;
            AudioManager.Instance.Play("Remove");
        }

        BuildManager.Instance.ToggleRemoveTool();
        rend.material.color = startColor;
    }

    private void HandleUpgrade()
    {
        if (plant == null)
        {
            Debug.Log("No plants to upgrade");
        }
        else
        {
            BuildManager.Instance.UpgradePlantOn(this);
        }
    }

    private void HandleBuild()
    {
        if (plant != null)
        {
            Debug.Log("Can't build here, space occupied");
        }
        else
        {
            BuildManager.Instance.BuildPlantOn(this);
        }
    }

    private void OnMouseEnter()
    {
        if (BuildManager.Instance.IsRemoveToolSelected)
        {
            rend.material.color = plant == null ? errorColor : hoverColor;
            return;
        }

        if (!BuildManager.Instance.CanBuild())
        {
            return;
        }

        if (BuildManager.Instance.HasMoney())
        {
            if (BuildManager.Instance.IsUpgrading)
            {
                rend.material.color = (plant == null || !isPlantUpgradeable) ? errorColor : hoverColor;
            }
            else
            {
                rend.material.color = plant != null ? errorColor : hoverColor;
            }
        }
        else
        {
            rend.material.color = errorColor;
        }
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}