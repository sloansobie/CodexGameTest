using System.Collections.Generic;
using UnityEngine;

public enum ImpCommandState
{
    FOCUS,
    DEFEND,
    SCATTER
}

public class MageImpManager : MonoBehaviour
{
    [SerializeField] private GameObject impPrefab;
    [SerializeField] private Transform summonOrigin;
    [SerializeField] private int maxActiveImps = 8;

    private readonly List<ImpAI> activeImps = new List<ImpAI>();
    private Transform markedTarget;

    public ImpCommandState CurrentCommandState { get; private set; } = ImpCommandState.DEFEND;
    public Transform MarkedTarget => markedTarget;

    public bool TrySummonImp()
    {
        CleanupDeadImps();
        if (activeImps.Count >= maxActiveImps)
        {
            return false;
        }

        Vector3 spawnPos = summonOrigin != null ? summonOrigin.position : transform.position + transform.forward;
        GameObject impObj = Instantiate(impPrefab, spawnPos, Quaternion.identity);
        ImpAI imp = impObj.GetComponent<ImpAI>();
        if (imp != null)
        {
            imp.Initialize(this);
            activeImps.Add(imp);
        }

        return true;
    }

    public void SetCommand(ImpCommandState state)
    {
        CurrentCommandState = state;
    }

    public void SetMarkedTarget(Transform target)
    {
        markedTarget = target;
    }

    public Vector3 GetDefendPoint()
    {
        return transform.position + transform.right * 2f;
    }

    private void CleanupDeadImps()
    {
        activeImps.RemoveAll(imp => imp == null);
    }
}
