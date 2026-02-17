using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MageImpManager : MonoBehaviour
{
    [Header("Summoning")]
    [SerializeField] private GameObject impPrefab;
    [SerializeField] private Transform mageTransform;
    [SerializeField] private Transform[] summonPoints;
    [SerializeField] private float summonCost = 20f;
    [SerializeField] private int maxActiveImps = 8;

    [Header("Resources")]
    [SerializeField] private float currentMana = 100f;

    [Header("UI")]
    [SerializeField] private Text impCountText;

    private readonly List<ImpAI> activeImps = new();

    public event Action<int, int> OnImpCountChanged;

    public IReadOnlyList<ImpAI> ActiveImps => activeImps;
    public float SummonCost => summonCost;
    public int MaxActiveImps => maxActiveImps;

    private void Start()
    {
        SyncImpCountUI();
    }

    public bool TrySummonImp()
    {
        if (impPrefab == null || mageTransform == null)
        {
            return false;
        }

        if (activeImps.Count >= maxActiveImps || currentMana < summonCost)
        {
            return false;
        }

        currentMana -= summonCost;

        Transform spawnPoint = GetSpawnPoint();
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : mageTransform.position;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : mageTransform.rotation;

        GameObject impObject = Instantiate(impPrefab, spawnPosition, spawnRotation);
        ImpAI imp = impObject.GetComponent<ImpAI>();

        if (imp == null)
        {
            Destroy(impObject);
            return false;
        }

        imp.Initialize(this, mageTransform);
        RegisterImp(imp);
        return true;
    }

    public void RegisterImp(ImpAI imp)
    {
        if (imp == null || activeImps.Contains(imp))
        {
            return;
        }

        activeImps.Add(imp);
        imp.OnDeath += HandleImpDeath;
        SyncImpCountUI();
    }

    public void UnregisterImp(ImpAI imp)
    {
        if (imp == null)
        {
            return;
        }

        imp.OnDeath -= HandleImpDeath;
        if (activeImps.Remove(imp))
        {
            SyncImpCountUI();
        }
    }

    private void HandleImpDeath(ImpAI imp)
    {
        UnregisterImp(imp);
    }

    private Transform GetSpawnPoint()
    {
        if (summonPoints == null || summonPoints.Length == 0)
        {
            return mageTransform;
        }

        return summonPoints[UnityEngine.Random.Range(0, summonPoints.Length)];
    }

    private void SyncImpCountUI()
    {
        OnImpCountChanged?.Invoke(activeImps.Count, maxActiveImps);

        if (impCountText != null)
        {
            impCountText.text = $"Imps: {activeImps.Count}/{maxActiveImps}";
        }
    }
}
