using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI statsText, statsText2;

    void Start()
    {
        statsText.text = "Parties jouées : " + PlayerPrefs.GetInt("gamesPlayed") + "\n"
                    + "Score total : " + PlayerPrefs.GetInt("totalScore") + "\n"
                    + "Temps en jeu : " + FormatPlayTime() + "\n"
                    + "Ennemis tués : " + PlayerPrefs.GetInt("enemiesKilled") + "\n"
                    + "Zizis tués : " + PlayerPrefs.GetInt("zizisKilled") + "\n"
                    + "Terroristes tués : " + PlayerPrefs.GetInt("terroristsKilled") + "\n"
                    + "Boss tués : " + PlayerPrefs.GetInt("bossKilled") + "\n";

        statsText2.text = "Items récupérés : " + PlayerPrefs.GetInt("itemsCollected") + "\n"
                    + "Nukes utilisées : " + PlayerPrefs.GetInt("nukesUsed") + "\n"
                    + "Body Pillows utilisés : " + PlayerPrefs.GetInt("pillowsUsed") + "\n"
                    + "Infections au Sida : " + PlayerPrefs.GetInt("sidaInfections") + "\n";
    }

    private string FormatPlayTime()
    {
        int totalSeconds = PlayerPrefs.GetInt("totalPlayTimeSeconds", 0);

        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;

        return $"{hours}h {minutes}m";
    }
}
