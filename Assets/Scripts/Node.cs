using UnityEngine;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color errorColor;
    [Header("HideInInspector")]
    public GameObject plant;
    [Header("HideInInspector")]
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
        if (BuildManager.Instance.isRemoveToolSelected)
        {
            if (plant != null)
            {
                Destroy(plant);
                isPlantUpgradeable = false;
                AudioManager.Instance.play("Remove");
            }

            BuildManager.Instance.removeToolClicked();
            rend.material.color = startColor;
        }

        if (!BuildManager.Instance.canBuild())
        {
            Debug.Log("plant is null");
            return;
        }

        if (BuildManager.Instance.isUpgrading)
        {
            if (plant == null)
            {
                // TODO : display on screen
                Debug.Log("No plants to upgrade");
            }
            else
            {
                // TODO : make sure this is the correct plant
                BuildManager.Instance.upgradePlantOn(this);
                rend.material.color = startColor;
            }
        }
        else
        {
            if (plant != null)
            {
                // TODO : display on screen
                Debug.Log("can't build here");
            }
            else
            {
                BuildManager.Instance.buildPlantOn(this);
                rend.material.color = startColor;
            }
        }

    }

    private void OnMouseEnter()
    {
        if (BuildManager.Instance.isRemoveToolSelected)
        {
            if (plant == null)
            {
                rend.material.color = errorColor;
            }
            else
            {
                rend.material.color = hoverColor;
            }

            return;
        }

        if (!BuildManager.Instance.canBuild())
        {
            return;
        }

        if (BuildManager.Instance.hasMoney())
        {
            if (BuildManager.Instance.isUpgrading)
            {
                if (plant == null || !isPlantUpgradeable)
                {
                    // TODO : check type of plant to upgrade
                    rend.material.color = errorColor;
                }
                else
                {
                    rend.material.color = hoverColor;
                }
            }
            else
            {
                if (plant != null)
                {
                    rend.material.color = errorColor;
                }
                else
                {
                    rend.material.color = hoverColor;
                }
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