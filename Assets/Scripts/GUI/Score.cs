using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score = 0;
    private TextMeshProUGUI text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "Score : 0";
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        text.text = "Score : " + score.ToString();
    }
}
