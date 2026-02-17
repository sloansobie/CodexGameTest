using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ImpAI : MonoBehaviour
{
    [SerializeField] private float scatterRadius = 6f;

    private MageImpManager manager;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Initialize(MageImpManager impManager)
    {
        manager = impManager;
    }

    private void Update()
    {
        if (manager == null)
        {
            return;
        }

        switch (manager.CurrentCommandState)
        {
            case ImpCommandState.FOCUS:
                if (manager.MarkedTarget != null)
                {
                    agent.SetDestination(manager.MarkedTarget.position);
                }
                break;
            case ImpCommandState.DEFEND:
                agent.SetDestination(manager.GetDefendPoint());
                break;
            case ImpCommandState.SCATTER:
                Vector3 randomOffset = Random.insideUnitSphere * scatterRadius;
                randomOffset.y = 0f;
                agent.SetDestination(manager.transform.position + randomOffset);
                break;
        }
    }
}
