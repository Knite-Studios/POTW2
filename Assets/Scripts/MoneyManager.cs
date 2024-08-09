using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : Singleton<MoneyManager>
{
    [HideInInspector]
    public int money;
    public Text moneyCounter;
    public int startMoney = 50;
    public int pointAmount = 25;
    
    private void Start()
    {
        money = startMoney;
        updateText();
    }

    public void useMoney(int amount)
    {
        money -= amount;
        updateText();
    }

    public void onPointClicked()
    {
        gainMoney(pointAmount);
        AudioManager.Instance.play("CollectPoint");
    }

    public void gainMoney(int amount)
    {
        money += amount;
        updateText();
    }

    private void updateText()
    {
        moneyCounter.text = "Hype = " + money;
    }
}