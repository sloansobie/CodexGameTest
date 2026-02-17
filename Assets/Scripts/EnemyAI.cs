using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public enum EnemyType
    {
        Thrall,
        HexArcher,
        StoneHusk
    }

    [SerializeField] private EnemyType enemyType;
    [SerializeField] private Transform target;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float rangedDistance = 10f;
    [SerializeField] private float huskPushDistance = 3f;

    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);
        switch (enemyType)
        {
            case EnemyType.Thrall:
                agent.SetDestination(target.position);
                if (distance <= meleeRange)
                {
                    Debug.Log("Thrall melee attack");
                }
                break;
            case EnemyType.HexArcher:
                HandleHexArcher(distance);
                break;
            case EnemyType.StoneHusk:
                HandleStoneHusk(distance);
                break;
        }
    }

    private void HandleHexArcher(float distance)
    {
        if (distance < rangedDistance * 0.75f)
        {
            Vector3 retreat = transform.position - (target.position - transform.position).normalized * 4f;
            agent.SetDestination(retreat);
        }
        else if (distance > rangedDistance)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.ResetPath();
            transform.LookAt(target);
            Debug.Log("Hex Archer ranged shot");
        }
    }

    private void HandleStoneHusk(float distance)
    {
        if (distance > meleeRange)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.ResetPath();
            Debug.Log("Stone Husk cleave");
            Vector3 pushDir = (target.position - transform.position).normalized;
            target.position += pushDir * huskPushDistance * Time.deltaTime;
        }
    }

    public void SetTarget(Transform nextTarget)
    {
        target = nextTarget;
    }
}
