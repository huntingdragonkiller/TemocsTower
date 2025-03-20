using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private int coins;

    public TextMeshProUGUI coinText;

    void Awake()
    {
        UpdateCoinText();
    }
    //adds coins, intended to be called when enemies die
    //TODO: play a small short coin sound
    public void AddCoins(int toAdd)
    {
        coins += toAdd;
        UpdateCoinText();
    }

    //Returns true if there are enough coins to spend, then subtracts from the stored coins
    //returns false if there are not enough coins
    //this function is intended to be used for purchasing upgrades for towers
    public bool SpendCoins(int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;
            Debug.Log("Purchase Successful (" + cost + " coins), remaining coins:" + coins);
            UpdateCoinText();
            return true;
        }
        return false;
    }

    void UpdateCoinText(){
        coinText.text = coins.ToString();
    }

}
