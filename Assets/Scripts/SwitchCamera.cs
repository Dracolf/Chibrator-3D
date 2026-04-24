using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject mainCamera, povCamera;
    int manager = 0;


    private void Start()
    {
        MainCam();
    }

    public void ManageCamera()
    {
        if (manager == 0)
        {
            PovCam();
            manager = 1;
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
    }

    private void PovCam()
    {
        mainCamera.SetActive(false);
        povCamera.SetActive(true);
    }

}
