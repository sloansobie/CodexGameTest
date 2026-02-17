using UnityEngine;
using UnityEngine.AI;

namespace CodexGame.Navigation
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavAgentConfigurator : MonoBehaviour
    {
        public enum AgentPreset
        {
            Enemy,
            Imp
        }

        [SerializeField] private AgentPreset preset;
        [SerializeField] private bool applyOnAwake = true;

        private NavMeshAgent agent;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            if (applyOnAwake)
            {
                ApplyPreset();
            }
        }

        [ContextMenu("Apply Preset")]
        public void ApplyPreset()
        {
            if (agent == null)
            {
                agent = GetComponent<NavMeshAgent>();
            }

            switch (preset)
            {
                case AgentPreset.Enemy:
                    agent.radius = 0.5f;
                    agent.height = 2f;
                    agent.speed = 3.5f;
                    agent.acceleration = 10f;
                    agent.angularSpeed = 500f;
                    break;
                case AgentPreset.Imp:
                    agent.radius = 0.35f;
                    agent.height = 1.2f;
                    agent.speed = 4.6f;
                    agent.acceleration = 14f;
                    agent.angularSpeed = 700f;
                    break;
            }
        }
    }
}
