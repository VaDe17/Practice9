using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursor : MonoBehaviour
{
    private PlayerBody playerBody;
    public Gradient gradient;
    public InputActionReference CursorAction;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        playerBody = PlayerBody.Instance;
    }

    void Update()
    {
        FollowMousePosition();

        UpdateDashChargeIndicator();

        if (CursorAction.action.triggered)
        {
            Debug.Log("Cursor Toggled");
            Cursor.visible = !Cursor.visible;
        }
    }

    private void FollowMousePosition()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + new Vector3 (0,0,10);
    }

    private void UpdateDashChargeIndicator()
    {
        float dashMultiplier = playerBody.GetPlayerDash.CurrentDashMultiplier();
        transform.localScale = new Vector3(dashMultiplier, dashMultiplier ,dashMultiplier) * 1.5f;

        RecolorIndicator(dashMultiplier);
    }

    private void RecolorIndicator(float value)
    {
        float t = (value - 1f) / (playerBody.GetStats.ChargedDashDistanceMultiplier - 1f);
        Color resultColor = gradient.Evaluate(t);

        spriteRenderer.color = resultColor;
    }
}
