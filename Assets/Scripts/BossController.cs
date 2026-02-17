using UnityEngine;

[RequireComponent(typeof(Health))]
public class BossController : MonoBehaviour
{
    [SerializeField] private float aegisMax = 200f;
    [SerializeField] private float aegisRegenPerSecond = 20f;
    [SerializeField] private float waveThreshold = 0.7f;
    [SerializeField] private GameObject addPrefab;
    [SerializeField] private Transform[] addSpawnPoints;

    private Health health;
    private EncounterController encounter;
    private float currentAegis;
    private bool vulnerable;
    private bool waveSpawned;

    private void Awake()
    {
        health = GetComponent<Health>();
        currentAegis = aegisMax;
    }

    private void Update()
    {
        if (encounter == null)
        {
            return;
        }

        if (!vulnerable)
        {
            currentAegis = Mathf.Min(aegisMax, currentAegis + aegisRegenPerSecond * Time.deltaTime);
        }

        if (!waveSpawned && health.CurrentHealth / Mathf.Max(1f, health.MaxHealth) <= waveThreshold)
        {
            SpawnAddWave();
            waveSpawned = true;
            vulnerable = false;
        }
    }

    public void BeginEncounter(EncounterController controller)
    {
        encounter = controller;
    }

    public void OnAddStateChanged(bool addsCleared)
    {
        vulnerable = addsCleared;
    }

    public void ApplyIncomingDamage(float amount, GameObject source)
    {
        if (!vulnerable)
        {
            currentAegis = Mathf.Max(0f, currentAegis - amount);
            return;
        }

        health.ApplyDamage(amount, source);
    }

    private void SpawnAddWave()
    {
        foreach (Transform spawn in addSpawnPoints)
        {
            if (spawn == null || addPrefab == null)
            {
                continue;
            }

            GameObject add = Instantiate(addPrefab, spawn.position, spawn.rotation);
            Health addHealth = add.GetComponent<Health>();
            if (addHealth != null)
            {
                encounter.RegisterAdd(addHealth);
            }
        }
    }
}
