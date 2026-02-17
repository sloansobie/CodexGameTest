using UnityEngine;

public class MageCommandSystem : MonoBehaviour
{
    [SerializeField] private MageImpManager impManager;
    [SerializeField] private Camera targetCamera;
    [SerializeField] private LayerMask targetMask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            impManager.SetCommand(ImpCommandState.FOCUS);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            impManager.SetCommand(ImpCommandState.DEFEND);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            impManager.SetCommand(ImpCommandState.SCATTER);
        }
    }

    public void MarkTargetFromScreenCenter()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 120f, targetMask))
        {
            impManager.SetMarkedTarget(hit.transform);
            impManager.SetCommand(ImpCommandState.FOCUS);
        }
    }
}
