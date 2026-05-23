using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private static WaitForSeconds _waitBeforeHitsInSeconds = new(0.5f);
    private static float _waitBetweenHitsInSeconds = 0.08f;

    private PlayerBody playerBody;
    private PlayerHealth playerHealth => playerBody.GetPlayerHealth;
    private PlayerMovement playerMovement => playerBody.GetPlayerMovement;
    private PlayerInput playerInput => playerBody.GetPlayerInput;


    [HideInInspector] public bool CanDash = true; 
    [HideInInspector] public bool IsChargingDash;

    private PlayerStats stats => playerBody.GetStats;
    private float dashDistance => stats.DashDistance;
    private float chargedDashDistanceMultiplier  => stats.ChargedDashDistanceMultiplier;
    private float maxChargeDuration => stats.MaxChargeDuration;
    private float dashDuration => stats.DashDuration;
    private float dashDamage => stats.DashDamage;
    private float dashCayoteTime => stats.DashCoyoteTime;

    private List<Collider2D> dashHits = new();
    private float currentDashChargeDuration;
    public float GetCurrentDashChargeDuration => currentDashChargeDuration;
    private bool cayoteDashQueued;
    private float cayoteTimer;

    // Events
    public event Action OnDashCharge;
    public event Action OnDashStart;
    public event Action<int> OnDashHit;
    public bool InDash {get; private set;}

    // Assets
    [Header("Dash Visual")]
    public GameObject DashVisual;

    // Experementals | WIP | Need to refactor
    [Header("EXPEREMENTAL FEATURES")]
    public bool SlowTimeOnDashCharge;
    public bool UseDashDistanceIndicator;

    public void Initialize(PlayerBody playerBody)
    {
        this.playerBody = playerBody;
    }

    private void Update()
    {
        UpdateTimers();

        playerBody.GetAnimator.SetBool("IsChargingDash", IsChargingDash);
    }

    private void UpdateTimers()
    {
        if ((currentDashChargeDuration < maxChargeDuration) && IsChargingDash)
            currentDashChargeDuration += Time.deltaTime;

        bool isChargeFull = currentDashChargeDuration >= maxChargeDuration;
        playerBody.GetAnimator.SetBool("IsChargeFull", isChargeFull);

        if (cayoteDashQueued)
        {
            cayoteTimer -= Time.deltaTime;

            if (cayoteTimer <= 0)
                cayoteDashQueued = false;
        }
    }

    public void Dash()
    {
        Vector2 dashDirection = (playerInput.GetMouseWorldPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 newDashDistance = dashDirection * (dashDistance * CurrentDashMultiplier());
        Vector3 targetPosition = transform.position + (Vector3)newDashDistance;

        float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;

        OnDashStart?.Invoke();
        SpawnDashVisual(angle);
        StartCoroutine(StartDash(targetPosition));
    }
    public void ChargeDash()
    {
        IsChargingDash = true;
        currentDashChargeDuration = 0;

        if (SlowTimeOnDashCharge)
        {
            Debug.Log("Start Slow");
            Time.timeScale = .4f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        OnDashCharge?.Invoke();
    }
    public void QueueCayoteDash()
    {
        cayoteDashQueued = true;
        cayoteTimer = dashCayoteTime;
    }

    private IEnumerator StartDash(Vector3 targetPosition)
    {
        Vector3 dashStartPosition = transform.position;
        playerBody.GetAnimator.SetBool("InDash", true);

        if (SlowTimeOnDashCharge)
        {
            Debug.Log("Reset Slow");
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        playerBody.GetMovementCollider.gameObject.layer = playerBody.DashGhostLayer;

        InDash = true;
        IsChargingDash = false;
        CanDash = false;
        playerMovement.CanMove = false;

        float totalDistance = Vector3.Distance(transform.position, targetPosition);
        float speedPerSecond = totalDistance / dashDuration;
        float elapsed = 0f;
        
        DashObstacleCheck(transform.position, targetPosition, playerBody.ObstacleMask, out var hit);
        Vector3 newTargetPosition = hit ? hit.point : targetPosition;

        DashAttackCheck(transform.position, newTargetPosition);

        while (elapsed < dashDuration)
        {
            elapsed += Time.deltaTime;

            Vector3 newPosition = Vector3.MoveTowards(
                transform.position,
                newTargetPosition,
                speedPerSecond * Time.deltaTime
            );

            transform.position = newPosition;

            yield return null;
        }
        
        DashEnd(dashStartPosition, newTargetPosition);
    }
    private void DashEnd(Vector3 dashStartPosition, Vector3 dashEndPosition)
    {
        DashDamageCheck();
        playerHealth.TakeDrainDashDamage();

        playerMovement.CanMove = true;
        CanDash = true;
        InDash = false;

        playerBody.GetMovementCollider.gameObject.layer = playerBody.PlayerLayer;
        currentDashChargeDuration = 0;

        // Animator + sprite stuff
        playerBody.GetAnimator.SetTrigger("DashEnd");
        playerBody.GetAnimator.SetBool("InDash", false);
        bool hasDashedRight = dashEndPosition.x - dashStartPosition.x > 0;
        playerBody.GetSprite.flipX = !hasDashedRight;

        if (cayoteDashQueued)
        {
            cayoteDashQueued = false;
            ChargeDash();
        }
    }

    public bool DashObstacleCheck(Vector3 from, Vector3 to, LayerMask obstacleLayers, out RaycastHit2D hit)
    {
        Vector2 start = from;
        Vector2 end = to;
        
        hit = Physics2D.Linecast(start, end, obstacleLayers);
        
        if (hit.collider != null)
            return false;
        else
            return true;
    }
    
    private void DashAttackCheck(Vector3 startPosition, Vector3 endPosition)
    {   
        ContactFilter2D filter = new();
        filter.SetLayerMask(playerBody.EnemiesMask);
        filter.useLayerMask = true;
        filter.useTriggers = false;
    
        Vector3 centrePosition = (startPosition + endPosition) / 2;
        Vector2 capsuleColliderSize = new(2.5f, Vector2.Distance(startPosition, endPosition) + 2.5f);

        Vector3 direction = endPosition - startPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
  
        Physics2D.OverlapCapsule(
            (Vector2)centrePosition,
            capsuleColliderSize,
            CapsuleDirection2D.Vertical,
            angle,
            filter, 
            dashHits
        );
    }
    private void SpawnDashVisual(float angle)
    {
        GameObject visual = Instantiate(DashVisual, transform.position, Quaternion.Euler(0, 0, angle));
        Destroy(visual, playerBody.GetStats.DashDuration * 2f);
    }

    public float CurrentDashMultiplier()
    {
        return Mathf.Lerp(1f, chargedDashDistanceMultiplier, currentDashChargeDuration / maxChargeDuration);
    }

    private void DashDamageCheck()
    {
        if(dashHits.Count == 0) 
            return;
        
        foreach (var hit in dashHits)
        {
            hit.GetComponentInParent<EnemyHealth>()?.AnticipateDamage(dashDamage);
        }

        OnDashHit?.Invoke(dashHits.Count);

        StartCoroutine(ProcessDashHitsWithDelay());
    }

    private IEnumerator ProcessDashHitsWithDelay()
    {
        var hitsToProcess = new List<Collider2D>(dashHits);
        dashHits.Clear();
        int hitsProcessed = 0;

        int totalHits = hitsToProcess.Count;
        hitsToProcess.Sort((a, b) => 
            Vector2.Distance(transform.position, b.transform.position).
            CompareTo(Vector2.Distance(transform.position, a.transform.position))
        );

        yield return _waitBeforeHitsInSeconds;

        foreach (var hit in hitsToProcess)
        {
            hit.GetComponentInParent<EnemyHealth>()?.TakeDamage(dashDamage);

            float timeBetweenHits = _waitBetweenHitsInSeconds * (1f + UnityEngine.Random.Range(-1f, 1f)) * (1f - (hitsProcessed * 0.02f));
            hitsProcessed++;

            yield return new WaitForSeconds(timeBetweenHits);
        }
    }
}
