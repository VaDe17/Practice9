using System.Collections.Generic;
using UnityEngine;

public class RestartManager : MonoBehaviour
{
    public static RestartManager Instance { get; private set; }

    private PlayerBody player;
    private List<EnemyBody> enemies = new();

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        player = PlayerBody.Instance;
    }

    public void AddEnemy(EnemyBody enemy)
    {
        enemies.Add(enemy);
    }

    public void Restart()
    {
        player.Restart();

        foreach (var enemy in enemies)
        {
            enemy.Restart();
        }
    }
}
