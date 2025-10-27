using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    public GameObject optionsPanel;  // The options panel to open/close

    public void PlayGame()
    {
        // Load the gameplay scene
        SceneManager.LoadScene("GameScene");
    }

    public void OpenOptions()
    {
        // Activate the options panel
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        // Close the options panel
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
#if UNITY_EDITOR
        // Stop play mode if running in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Close the application if built
        Application.Quit();
#endif
    }
}
