using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerInput : MonoBehaviour
{
    private PlayerBody playerBody;
    //private PlayerMovement playerMovement => playerBody.GetPlayerMovement;
    private PlayerDash playerDash => playerBody.GetPlayerDash;

    public InputActionReference MoveAction;
    public InputActionReference DashAction;

    public Vector2 GetMouseWorldPosition => playerBody.GetPlayerCamera.GetCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

    public void Initialize(PlayerBody playerBody)
    {
        this.playerBody = playerBody;
    }

    private void Update()
    {
        if (playerDash.CanDash && !playerDash.IsChargingDash && DashAction.action.WasPressedThisFrame())
        {
            playerDash.ChargeDash();
        } 
        else if (!playerDash.CanDash && DashAction.action.WasPressedThisFrame())
        {
            playerDash.QueueCayoteDash();
        }

        if (playerDash.IsChargingDash && DashAction.action.WasReleasedThisFrame())
        {
            playerDash.Dash();
        }

        // Set Animator Bool
        bool isMoving = !MoveAction.action.ReadValue<Vector2>().Equals(Vector2.zero);
        playerBody.GetAnimator.SetBool("IsMoving", isMoving);

        // Flip Sprite depending on mousePosition, during dashCharge
        if (playerBody.GetPlayerDash.IsChargingDash)
        {
            bool mouseOnTheRightSide = GetMouseWorldPosition.x - transform.position.x > 0;
            playerBody.GetSprite.flipX = !mouseOnTheRightSide;
        } else
        {                
            // Flip Sprite depending on move input
            if (MoveAction.action.ReadValue<Vector2>().x > 0)
                playerBody.GetSprite.flipX = false;
            else if (MoveAction.action.ReadValue<Vector2>().x < 0)
                playerBody.GetSprite.flipX = true;
        }
    }
}
