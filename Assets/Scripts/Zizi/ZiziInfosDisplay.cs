using TMPro;
using UnityEngine;

public class ZiziInfosDisplay : MonoBehaviour
{
    private TextMeshPro text;
    private ZiziController ziziController;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshPro>();
        ziziController = GetComponent<ZiziController>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "speed : " + ziziController.speed.ToString();
    }
}
