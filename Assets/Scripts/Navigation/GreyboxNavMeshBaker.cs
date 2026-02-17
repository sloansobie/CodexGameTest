using UnityEngine;

#if UNITY_AI_NAVIGATION
using Unity.AI.Navigation;
#endif

namespace CodexGame.Navigation
{
    public class GreyboxNavMeshBaker : MonoBehaviour
    {
#if UNITY_AI_NAVIGATION
        [SerializeField] private NavMeshSurface navMeshSurface;
#endif

        [ContextMenu("Bake Greybox NavMesh")]
        public void Bake()
        {
#if UNITY_AI_NAVIGATION
            if (navMeshSurface == null)
            {
                navMeshSurface = GetComponent<NavMeshSurface>();
            }

            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
            }
#else
            Debug.LogWarning("GreyboxNavMeshBaker requires the AI Navigation package and UNITY_AI_NAVIGATION scripting define.");
#endif
        }
    }
}
