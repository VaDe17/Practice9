using UnityEngine;
public struct TransitionCamera
{
    public Vector2 WorldPosition;
    public float CameraSize;
    public float TransitionDuration;

    public TransitionCamera(Vector2 worldPos, float camSize, float duration)
    {
        WorldPosition = worldPos;
        CameraSize = camSize;
        TransitionDuration = duration;
    }
}
public class CameraPosition : MonoBehaviour
{
    [SerializeField] private Collider2D trigger;
    public float CameraSize = 8;
    public float TransitionDuration = 1f;
    
    private Transform player;
    private PlayerCamera playerCamera;
    private TransitionCamera thisCamera;

    private void Start()
    {
        player = PlayerBody.Instance.transform;

        playerCamera = PlayerBody.Instance.GetPlayerCamera;

        thisCamera = new((Vector2)transform.position, CameraSize, TransitionDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.parent.transform == player)
        {
            playerCamera.DoTransition(thisCamera);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.transform.parent.transform == player)
        {
            playerCamera.CheckExit(thisCamera);
        }
    }
}
