using UnityEngine;

public class MageCommandSystem : MonoBehaviour
{
    public enum MageCommand
    {
        FOCUS,
        DEFEND,
        SCATTER
    }

    [Header("References")]
    [SerializeField] private MageImpManager impManager;

    [Header("Focus Marker")]
    [SerializeField] private GameObject focusMarkerPrefab;

    private MageCommand activeCommand = MageCommand.DEFEND;
    private Transform focusTarget;
    private GameObject focusMarkerInstance;

    private void Update()
    {
        UpdateFocusMarkerPosition();
    }

    public void IssueCommand(MageCommand command, Transform target = null)
    {
        activeCommand = command;

        switch (command)
        {
            case MageCommand.FOCUS:
                SetFocusTarget(target);
                BroadcastImpCommand(ImpAI.ImpCommand.Focus, focusTarget);
                break;
            case MageCommand.DEFEND:
                ClearFocusTarget();
                BroadcastImpCommand(ImpAI.ImpCommand.Defend);
                break;
            case MageCommand.SCATTER:
                ClearFocusTarget();
                BroadcastImpCommand(ImpAI.ImpCommand.Scatter);
                break;
        }
    }

    public void SetFocusTarget(Transform target)
    {
        focusTarget = target;

        if (focusTarget == null)
        {
            HideFocusMarker();
            return;
        }

        if (focusMarkerPrefab != null && focusMarkerInstance == null)
        {
            focusMarkerInstance = Instantiate(focusMarkerPrefab);
        }

        UpdateFocusMarkerPosition();
    }

    private void ClearFocusTarget()
    {
        focusTarget = null;
        HideFocusMarker();
    }

    private void BroadcastImpCommand(ImpAI.ImpCommand impCommand, Transform target = null)
    {
        if (impManager == null)
        {
            return;
        }

        foreach (ImpAI imp in impManager.ActiveImps)
        {
            if (imp == null)
            {
                continue;
            }

            imp.ApplyCommand(impCommand, target);
        }
    }

    private void UpdateFocusMarkerPosition()
    {
        if (activeCommand != MageCommand.FOCUS || focusMarkerInstance == null || focusTarget == null)
        {
            return;
        }

        focusMarkerInstance.transform.position = focusTarget.position;
    }

    private void HideFocusMarker()
    {
        if (focusMarkerInstance != null)
        {
            Destroy(focusMarkerInstance);
            focusMarkerInstance = null;
        }
    }
}
