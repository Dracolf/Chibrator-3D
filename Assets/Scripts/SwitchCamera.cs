using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject mainCamera, povCamera, backCamera, skyCamera;
    int manager = 0;


    private void Start()
    {
        MainCam();
    }

    public void ManageCamera()
    {
        if (RastaCameraManager.Instance != null && RastaCameraManager.Instance.IsLockedOnRasta)
        {
            return;
        }
        if (manager == 0)
        {
            PovCam();
            manager = 1;
        } else if (manager == 1)
        {
            BackCam();
            manager = 2;
        } else if (manager == 2)
        {
            SkyCam();
            manager = 3;
        } else
        {
            MainCam();
            manager = 0;
        }  
    }

    private void MainCam()
    {
        mainCamera.SetActive(true);
        povCamera.SetActive(false);
        backCamera.SetActive(false);
        skyCamera.SetActive(false);
    }

    private void PovCam()
    {
        mainCamera.SetActive(false);
        povCamera.SetActive(true);
        backCamera.SetActive(false);
        skyCamera.SetActive(false);
    }

    private void BackCam()
    {
        mainCamera.SetActive(false);
        povCamera.SetActive(false);
        backCamera.SetActive(true);
        skyCamera.SetActive(false);
    }

    private void SkyCam()
    {
        mainCamera.SetActive(false);
        povCamera.SetActive(false);
        backCamera.SetActive(false);
        skyCamera.SetActive(true);
    }

}
