using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            CloseApp();
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(3))
        {
            CloseApp();
        }
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
