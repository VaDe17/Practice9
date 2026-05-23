using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerBody playerBody;
    private PlayerInput playerInput => playerBody.GetPlayerInput;
    private PlayerDash playerDash => playerBody.GetPlayerDash;

    private PlayerStats stats => playerBody.GetStats;
    private Rigidbody2D rigidBody => playerBody.GetRigidBody2D;

    private float moveSpeed;
    private float chargingMoveSpeedMultiplier;

    private float adjustedMoveSpeed => moveSpeed * rigidBody.linearDamping;

    [HideInInspector] public bool CanMove = true;


    public void Initialize(PlayerBody playerBody)
    {
        this.playerBody = playerBody;

        moveSpeed = stats.MoveSpeed;
        chargingMoveSpeedMultiplier = stats.ChargingMoveSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            float moveSpeed = playerDash.IsChargingDash ? adjustedMoveSpeed * chargingMoveSpeedMultiplier : adjustedMoveSpeed;
            Vector2 force = playerInput.MoveAction.action.ReadValue<Vector2>() * moveSpeed;
            rigidBody.AddForce(force);
        }
    }
}