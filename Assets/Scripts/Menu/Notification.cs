using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField]
    private Sprite stoneSprite, goldSprite;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Image image;

    private string skinName;

    public void SetSkin(string SkinName)
    {
        skinName = SkinName;

        if (skinName == "stone")
        {
            image.sprite = stoneSprite;
            text.text = "Tu as débloqué le skin : Statue";
        }
        if (skinName == "gold")
        {
            image.sprite = goldSprite;
            text.text = "Tu as débloqué le skin : Golden";
        }

        NotificationManager.notification = "";
    }

}
