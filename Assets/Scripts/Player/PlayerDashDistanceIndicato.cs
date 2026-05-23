using UnityEngine;

public class PlayerDashDistanceIndicator : MonoBehaviour
{
    public SpriteRenderer Marker;
    private Transform markerTransform => Marker.transform;
    private PlayerBody playerBody => PlayerBody.Instance;
    private PlayerDash playerDash => playerBody.GetPlayerDash;
    private PlayerInput playerInput => playerBody.GetPlayerInput;
    private PlayerStats playerStats => playerBody.GetStats;

    private void Update()
    {
        if(!playerDash.UseDashDistanceIndicator)
        {
            Marker.enabled = false;
            return;
        }

        if (!playerDash.IsChargingDash)
        {
            Marker.enabled = false;
            return;
        }

        Marker.enabled = true;

        Vector3 direction = (playerInput.GetMouseWorldPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 newDashDistance = direction * (playerStats.DashDistance * playerDash.CurrentDashMultiplier());
        Vector3 targetPosition = transform.position + (Vector3)newDashDistance;
        playerDash.DashObstacleCheck(transform.position, targetPosition, playerBody.ObstacleMask, out var hit);

        Vector3 newTargetPosition = hit ? hit.point : targetPosition;
        markerTransform.position = newTargetPosition;
    }
}
