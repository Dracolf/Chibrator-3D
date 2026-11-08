using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Customisation : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer[] characterMeshes;
    [SerializeField]
    private Material baseMaterial, stoneMaterial, goldMaterial;
    [SerializeField]
    private SoundEffectPlayer soundEffectPlayer;
    [SerializeField]
    private Image lock1, lock2;
    [SerializeField]
    private Button stoneSkinButtton, goldSkinButton;
    [SerializeField]
    private TextMeshProUGUI stoneSkinText, goldSkinText;

    private bool isStoneSkinUnlocked, isGoldSkinUnlocked;

    private void Awake()
    {
        isStoneSkinUnlocked = PlayerPrefs.GetInt("HighScore") >= 5000;
        lock1.gameObject.SetActive(!isStoneSkinUnlocked);
        stoneSkinButtton.interactable = isStoneSkinUnlocked;
        stoneSkinText.gameObject.SetActive(isStoneSkinUnlocked);

        isGoldSkinUnlocked = PlayerPrefs.GetInt("HighScore") >= 10000;
        lock2.gameObject.SetActive(!isGoldSkinUnlocked);
        goldSkinButton.interactable = isGoldSkinUnlocked;
        goldSkinText.gameObject.SetActive(isGoldSkinUnlocked);
    }

    public void ApplyBaseSkin()
    {
        foreach (SkinnedMeshRenderer mesh in characterMeshes)
        {
            mesh.sharedMaterial = baseMaterial;
            PlayerPrefs.SetString("CurrentSkin", "Base");
            PlayerPrefs.Save();           
            soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);
        }
    }

    public void ApplyStoneSkin()
    {
        foreach (SkinnedMeshRenderer mesh in characterMeshes)
        {
            mesh.sharedMaterial = stoneMaterial;
            PlayerPrefs.SetString("CurrentSkin", "Stone");
            PlayerPrefs.Save();
            soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);

        }
    }

    public void ApplyGoldSkin()
    {
        foreach (SkinnedMeshRenderer mesh in characterMeshes)
        {
            mesh.sharedMaterial = goldMaterial;
            PlayerPrefs.SetString("CurrentSkin", "Gold");
            PlayerPrefs.Save();
            soundEffectPlayer.PlaySound(SoundEffectType.SoftProut);
        }
    }
}
