using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeath : MonoBehaviour
{
    private void OnTriggerEnter(Collider player)
    {
        SceneManager.LoadScene("Menu");
    }
}
