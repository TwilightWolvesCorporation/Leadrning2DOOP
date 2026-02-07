using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene($"Level_1");
    }

    public void Exit()
    {
        Application.Quit();
    }
}