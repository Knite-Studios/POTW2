using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private Text moneyCounter;
    [SerializeField] private int startMoney = 50;

    public int Money { get; private set; }

    private void Start()
    {
        Money = startMoney;
        UpdateMoneyDisplay();
    }

    public void UseMoney(int amount)
    {
        Money -= amount;
        UpdateMoneyDisplay();
    }

    public void CollectHype(int amount)
    {
        Money += amount;
        UpdateMoneyDisplay();
        AudioManager.Instance.Play("CollectHype");
    }

    private void UpdateMoneyDisplay()
    {
        if (moneyCounter != null)
        {
            moneyCounter.text = $"Hype = {Money}";
        }
        else
        {
            Debug.LogWarning("Money counter Text component is not assigned!");
        }
    }
}