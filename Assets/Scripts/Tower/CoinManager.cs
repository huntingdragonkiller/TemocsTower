using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private int coins;

    //adds coins, intended to be called when enemies die
    //TODO: play a small short coin sound
    public void AddCoins(int toAdd)
    {
        coins += toAdd;
    }

    //Returns true if there are enough coins to spend, then subtracts from the stored coins
    //returns false if there are not enough coins
    //this function is intended to be used for purchasing upgrades for towers
    public bool SpendCoins(int cost)
    {
        if (coins >= cost)
        {
            coins -= cost;
            return true;
        }
        return false;
    }

}
