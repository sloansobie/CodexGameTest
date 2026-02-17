using System;
using UnityEngine;

public class TeamResourceManager : MonoBehaviour
{
    [SerializeField] private float sigilsPerDamage = 0.1f;
    [SerializeField] private float maxSigils = 100f;

    public float Sigils { get; private set; }

    public event Action<float> SigilsChanged;

    public void RegisterTeamDamage(float amount)
    {
        if (amount <= 0f)
        {
            return;
        }

        Sigils = Mathf.Min(maxSigils, Sigils + amount * sigilsPerDamage);
        SigilsChanged?.Invoke(Sigils);
    }

    public bool CanSpend(float cost)
    {
        return cost > 0f && Sigils >= cost;
    }

    public bool TrySpend(float cost)
    {
        if (!CanSpend(cost))
        {
            return false;
        }

        Sigils -= cost;
        SigilsChanged?.Invoke(Sigils);
        return true;
    }
}
