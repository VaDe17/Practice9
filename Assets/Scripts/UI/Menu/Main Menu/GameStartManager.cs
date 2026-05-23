using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public void StartNewGame()
    {
        BasicSceneManager.Instance.LoadScene("Level1");
    }
}
