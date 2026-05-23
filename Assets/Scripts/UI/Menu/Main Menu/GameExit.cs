using UnityEngine;
using UnityEngine.UI; // Required to work with UI Buttons

public class GameExit : MonoBehaviour
{
    // Reference to a UI Panel that holds your "Yes/No" buttons
    //public GameObject quitConfirmationPanel; 

    // void Start()
    // {
    //     // Automatically hide the confirmation panel when the game starts
    //     if (quitConfirmationPanel != null)
    //         quitConfirmationPanel.SetActive(false);
    // }

    // Call this function when the user clicks your "Quit" button
    // public void OnQuitButtonPressed()
    // {
    //     // For PC / Mac builds, show a confirmation dialog first.
    //     // For other platforms, you might want to quit directly.
    //     #if UNITY_STANDALONE
    //         ShowConfirmationPanel(true);
    //     #else
    //         QuitGame();
    //     #endif
    // }

    // Called by the "Yes" button on the confirmation panel
    public void ConfirmQuit()
    {
        QuitGame();
    }

    // Called by the "No" button on the confirmation panel
    // public void CancelQuit()
    // {
    //     ShowConfirmationPanel(false);
    // }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            // This stops Play Mode in the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // This quits the built application (Windows, Mac, Linux, Android)
            Application.Quit();
        #endif
    }

    // private void ShowConfirmationPanel(bool show)
    // {
    //     if (quitConfirmationPanel != null)
    //     {
    //         quitConfirmationPanel.SetActive(show);
    //         // Optionally, stop game time when the panel is shown
    //         // Time.timeScale = show ? 0f : 1f;
    //     }
    // }
}