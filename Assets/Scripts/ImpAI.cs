using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ImpAI : MonoBehaviour, IDamageable
{
    public enum ImpCommand
    {
        Follow,
        Focus,
        Defend,
        Scatter
    }

    [Header("Survivability")]
    [SerializeField] private float maxHealth = 30f;

    [Header("Combat")]
    [SerializeField] private LayerMask hostileLayers;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.25f;

    [Header("Movement")]
    [SerializeField] private float followDistance = 2.5f;
    [SerializeField] private float scatterDistance = 5f;

    private MageImpManager manager;
    private Transform mageTransform;
    private NavMeshAgent agent;
    private Transform commandedTarget;
    private Transform acquiredTarget;
    private ImpCommand currentCommand = ImpCommand.Follow;
    private float currentHealth;
    private float attackTimer;

    public event Action<ImpAI> OnDeath;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (mageTransform == null)
        {
            return;
        }

        attackTimer -= Time.deltaTime;

        AcquireTargetIfNeeded();
        Transform activeTarget = GetActiveTarget();

        if (activeTarget != null)
        {
            float distance = Vector3.Distance(transform.position, activeTarget.position);
            if (distance <= attackRange)
            {
                agent.ResetPath();
                TryAttack(activeTarget);
                return;
            }

            agent.SetDestination(activeTarget.position);
            return;
        }

        ExecuteCommandMovement();
    }

    public void Initialize(MageImpManager impManager, Transform ownerMage)
    {
        manager = impManager;
        mageTransform = ownerMage;
    }

    public void ApplyCommand(ImpCommand command, Transform target = null)
    {
        currentCommand = command;
        commandedTarget = target;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void AcquireTargetIfNeeded()
    {
        if (currentCommand == ImpCommand.Focus && commandedTarget != null)
        {
            acquiredTarget = commandedTarget;
            return;
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, hostileLayers);

        float closestDistance = float.MaxValue;
        Transform bestTarget = null;

        foreach (Collider hit in hits)
        {
            float distance = (hit.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestTarget = hit.transform;
            }
        }

        acquiredTarget = bestTarget;
    }

    private Transform GetActiveTarget()
    {
        if (currentCommand == ImpCommand.Focus && commandedTarget != null)
        {
            return commandedTarget;
        }

        return acquiredTarget;
    }

    private void ExecuteCommandMovement()
    {
        switch (currentCommand)
        {
            case ImpCommand.Defend:
            case ImpCommand.Follow:
                FollowMage();
                break;
            case ImpCommand.Scatter:
                ScatterFromMage();
                break;
            case ImpCommand.Focus:
                if (commandedTarget == null)
                {
                    FollowMage();
                }
                break;
        }
    }

    private void FollowMage()
    {
        float distance = Vector3.Distance(transform.position, mageTransform.position);
        if (distance > followDistance)
        {
            agent.SetDestination(mageTransform.position);
        }
        else
        {
            agent.ResetPath();
        }
    }

    private void ScatterFromMage()
    {
        Vector3 awayDirection = (transform.position - mageTransform.position).normalized;
        if (awayDirection == Vector3.zero)
        {
            awayDirection = UnityEngine.Random.insideUnitSphere;
            awayDirection.y = 0f;
            awayDirection.Normalize();
        }

        Vector3 scatterPoint = mageTransform.position + awayDirection * scatterDistance;
        agent.SetDestination(scatterPoint);
    }

    private void TryAttack(Transform target)
    {
        if (attackTimer > 0f)
        {
            return;
        }

        attackTimer = attackCooldown;

        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(attackDamage);
    }

    private void Die()
    {
        OnDeath?.Invoke(this);
        manager?.UnregisterImp(this);
        Destroy(gameObject);
    }
}
