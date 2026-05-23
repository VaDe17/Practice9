using UnityEngine;
using UnityEngine.InputSystem;

public class DebugEnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            Debug.Log("Spawing enemy");
            
            Vector3 spawnPosition = (Vector3)PlayerBody.Instance.GetPlayerInput.GetMouseWorldPosition + new Vector3 (0,0,10);
            Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
        }

        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            Debug.Log("Heal player to max");
            
            PlayerBody.Instance.GetPlayerHealth.HealHealth(PlayerBody.Instance.GetPlayerHealth.MaxHealth);
        }

        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            Debug.Log("Teleport to cursor position");
            
            PlayerBody.Instance.transform.position = PlayerBody.Instance.GetPlayerInput.GetMouseWorldPosition;
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("Restarting the game");
            
            RestartManager.Instance.Restart();
        }

        if (Keyboard.current.f1Key.wasPressedThisFrame)
        {
            Debug.Log("Toggle DEBUG mode");
            
            DebugMode.Instance.IsDebugModeActive = !DebugMode.Instance.IsDebugModeActive;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("Exit to menu");
            
            BasicSceneManager.Instance.LoadScene("MainMenu");
        }
    }
}
