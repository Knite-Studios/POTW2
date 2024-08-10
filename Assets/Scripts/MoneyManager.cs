using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{

    public static MoneyManager instance;

    [HideInInspector]
    public int Money;
    public Text moneyCounter;
    public int startMoney = 50;
    public int pointAmount = 25;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one moeny manager in the scene!");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        Money = startMoney;
        updateText();
    }

    public void useMoney(int amount)
    {
        Money -= amount;
        updateText();
    }

    public void onPointClicked()
    {
        gainMoney(pointAmount);
        AudioManager.instance.play("CollectPoint");
    }

    public void gainMoney(int amount)
    {
        Money += amount;
        updateText();
    }

    private void updateText()
    {
        moneyCounter.text = "Hype = " + Money;
    }
}