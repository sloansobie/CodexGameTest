using System.Collections.Generic;
using UnityEngine;

public class EncounterController : MonoBehaviour
{
    [SerializeField] private BossController bossController;
    [SerializeField] private List<Health> activeAdds = new List<Health>();

    public bool AreAddsCleared => activeAdds.Count == 0;

    private void OnEnable()
    {
        foreach (Health add in activeAdds)
        {
            if (add != null)
            {
                add.Died += OnAddDied;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Health add in activeAdds)
        {
            if (add != null)
            {
                add.Died -= OnAddDied;
            }
        }
    }

    public void StartEncounter()
    {
        bossController.BeginEncounter(this);
    }

    public void RegisterAdd(Health add)
    {
        if (add == null)
        {
            return;
        }

        activeAdds.Add(add);
        add.Died += OnAddDied;
    }

    private void OnAddDied(Health add)
    {
        add.Died -= OnAddDied;
        activeAdds.Remove(add);
        bossController.OnAddStateChanged(AreAddsCleared);
    }
}
