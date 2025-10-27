using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject optionsPanel;

    void Start()
    {
        // Make cursor visible in the main menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Called by the "Continue" button
    public void PlayGame()
    {
        // Load the gameplay scene (make sure the name matches exactly)
        SceneManager.LoadScene("GameScene");
    }

    // Called by "Options" button
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    // Called by the "Back" button inside the options panel
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    // Called by "Quit" button
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
