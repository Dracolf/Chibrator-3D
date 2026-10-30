using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private GameObject scoringDisabled;
    public int score = 0;
    private TextMeshProUGUI text;
    private int chinoisCount = 0;
    public bool isScoringEnabled = true;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Score : 0";
    }

    public void IncreaseScore(int amount)
    {
        if (isScoringEnabled)
        {
            score += amount;
            text.text = "Score : " + score.ToString();
        }
    }

    public void IncreaseChinoisCount()
    {
        chinoisCount++;
        ToggleScoring();
    }

    public void DecreaseChinoisCount()
    {
        chinoisCount--;
        ToggleScoring();
    }

    private void ToggleScoring()
    {
        if (chinoisCount > 0) 
        {
            text.enabled = false;
            scoringDisabled.SetActive(true);
            isScoringEnabled = false;
            return;
        }
        text.enabled = true;
        scoringDisabled.SetActive(false);
        isScoringEnabled = true;
        return;
    }
}
