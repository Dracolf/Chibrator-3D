using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText, nukeAmountText, pillowAmountText, leurreAmountText, notEnough;
    private int money, nukeAmount = 0, pillowAmount = 0, leurreAmount = 0;
    private readonly int bigPrices = 10000;
    private readonly int leurrePrice = 6500;
    private SoundEffectPlayer soundEffectPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money = PlayerPrefs.GetInt("money");
        moneyText.text = money.ToString();
        nukeAmount = PlayerPrefs.GetInt("nukeAmount");
        nukeAmountText.text = nukeAmount.ToString();
        pillowAmount = PlayerPrefs.GetInt("pillowAmount");
        pillowAmountText.text = pillowAmount.ToString();
        leurreAmount = PlayerPrefs.GetInt("leurreAmount");
        leurreAmountText.text = leurreAmount.ToString();
        notEnough.gameObject.SetActive(false);
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
    }

    public void BuyNuke()
    {
        if (money - bigPrices >= 0)
        {
            money -= bigPrices;
            PlayerPrefs.SetInt("money", money);
            moneyText.text = money.ToString();
            nukeAmount ++;
            PlayerPrefs.SetInt("nukeAmount", nukeAmount);
            nukeAmountText.text = nukeAmount.ToString();

            soundEffectPlayer.PlaySound(SoundEffectType.Buy);
            PlayerPrefs.Save();
        }
        else
        {
            notEnough.gameObject.SetActive(true);
            soundEffectPlayer.PlaySound(SoundEffectType.Error);
        }
    }

    public void BuyPillow()
    {
        if (money - bigPrices >= 0)
        {
            money -= bigPrices;
            PlayerPrefs.SetInt("money", money);
            moneyText.text = money.ToString();
            pillowAmount ++;
            PlayerPrefs.SetInt("pillowAmount", pillowAmount);
            pillowAmountText.text = pillowAmount.ToString();

            soundEffectPlayer.PlaySound(SoundEffectType.Buy);
            PlayerPrefs.Save();
        }
        else
        {
            notEnough.gameObject.SetActive(true);
            soundEffectPlayer.PlaySound(SoundEffectType.Error);
        }
    }

    public void BuyLeurre()
    {
        if (money - leurrePrice >= 0)
        {
            money -= leurrePrice;
            PlayerPrefs.SetInt("money", money);
            moneyText.text = money.ToString();
            leurreAmount ++;
            PlayerPrefs.SetInt("leurreAmount", leurreAmount);
            leurreAmountText.text = leurreAmount.ToString();

            soundEffectPlayer.PlaySound(SoundEffectType.Buy);
            PlayerPrefs.Save();
        }
        else
        {
            notEnough.gameObject.SetActive(true);
            soundEffectPlayer.PlaySound(SoundEffectType.Error);
        }
    }
}
