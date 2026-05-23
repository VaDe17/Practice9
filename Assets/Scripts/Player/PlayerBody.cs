using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Player Singleton - for easy accsess by other scripts
    public static PlayerBody Instance {get; private set;}

    [Header("Player Stats Reference")]
    [SerializeField] private PlayerStats playerStats;
    public PlayerStats GetStats => playerStats;

    [Header("Player Scripts References")]
    public PlayerHealth GetPlayerHealth;
    public PlayerInput GetPlayerInput;
    public PlayerMovement GetPlayerMovement;
    public PlayerDash GetPlayerDash;
    public PlayerAudio GetPlayerAudio;
    [HideInInspector] public PlayerCamera GetPlayerCamera;

    [Header("Components References")]
    public Collider2D GetMovementCollider;
    public Rigidbody2D GetRigidBody2D;
    public Animator GetAnimator;
    public SpriteRenderer GetSprite;

    [Header("Masks References")]
    public LayerMask ObstacleMask;
    public LayerMask EnemiesMask;
    public int DashGhostLayer => LayerMask.NameToLayer("GhostPlayer");
    public int PlayerLayer => LayerMask.NameToLayer("Player");

    private Vector3 startPostion;

    private void Awake()
    {
        Instance = this;
        GetPlayerCamera = Camera.main.GetComponent<PlayerCamera>();
    }

    private void Start()
    {
        startPostion = transform.position;

        GetPlayerHealth.Initialize(this);
        GetPlayerMovement.Initialize(this);
        GetPlayerDash.Initialize(this);
        GetPlayerInput.Initialize(this);
        
        GetPlayerAudio.Initialize(this);
    }

    public void Restart()
    {
        transform.position = startPostion;
        GetPlayerDash.StopAllCoroutines();
        GetPlayerDash.CanDash = true;
        GetPlayerMovement.CanMove = true;
        GetPlayerHealth.HealHealth(9999);
    }
}
