using System;
using System.Collections;
using System.Collections.Generic;
using CodexGame.Enemies;
using UnityEngine;

namespace CodexGame.Boss
{
    public class BossController : MonoBehaviour
    {
        [Header("Vitals")]
        [SerializeField] private float maxHealth = 2000f;
        [SerializeField] private float maxAegis = 400f;
        [SerializeField] private float aegisRegenPerSecond = 20f;
        [SerializeField] private float aegisRegenDelay = 5f;

        [Header("Summons")]
        [SerializeField] private EnemyActor thrallPrefab;
        [SerializeField] private EnemyActor hexArcherPrefab;
        [SerializeField] private Transform[] summonPoints;
        [SerializeField] private float thrallSummonInterval = 20f;
        [SerializeField] private int thrallsPerSummon = 6;
        [SerializeField] private int hexArchersOnHalfHealth = 4;

        [Header("Pressure")]
        [SerializeField] private int pressureAddThreshold = 3;

        public event Action<bool> BossBarVisibilityChanged;
        public event Action<float, float, float, float, bool> BossVitalsChanged;
        public event Action Defeated;

        private readonly List<EnemyActor> liveAdds = new();
        private float health;
        private float aegis;
        private float lastDamageTime;
        private bool halfHealthWaveSpawned;
        private bool active;

        public bool IsVulnerable => liveAdds.Count < pressureAddThreshold;

        private void Awake()
        {
            health = maxHealth;
            aegis = maxAegis;
        }

        private void OnEnable()
        {
            StartBossFight();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            BossBarVisibilityChanged?.Invoke(false);
        }

        private void Update()
        {
            if (!active)
            {
                return;
            }

            if (Time.time - lastDamageTime >= aegisRegenDelay)
            {
                aegis = Mathf.Min(maxAegis, aegis + (aegisRegenPerSecond * Time.deltaTime));
                PublishVitals();
            }
        }

        public void StartBossFight()
        {
            if (active)
            {
                return;
            }

            active = true;
            BossBarVisibilityChanged?.Invoke(true);
            PublishVitals();
            StartCoroutine(ThrallSummonLoop());
        }

        public void TakeDamage(float amount)
        {
            if (!active || amount <= 0f)
            {
                return;
            }

            lastDamageTime = Time.time;

            if (aegis > 0f)
            {
                aegis = Mathf.Max(0f, aegis - amount);
                PublishVitals();
                return;
            }

            if (!IsVulnerable)
            {
                return;
            }

            health = Mathf.Max(0f, health - amount);
            if (!halfHealthWaveSpawned && health <= maxHealth * 0.5f)
            {
                halfHealthWaveSpawned = true;
                SpawnWave(hexArcherPrefab, hexArchersOnHalfHealth);
            }

            PublishVitals();

            if (health <= 0f)
            {
                active = false;
                StopAllCoroutines();
                BossBarVisibilityChanged?.Invoke(false);
                Defeated?.Invoke();
            }
        }

        private IEnumerator ThrallSummonLoop()
        {
            while (active)
            {
                SpawnWave(thrallPrefab, thrallsPerSummon);
                PublishVitals();
                yield return new WaitForSeconds(thrallSummonInterval);
            }
        }

        private void SpawnWave(EnemyActor prefab, int count)
        {
            if (prefab == null || summonPoints == null || summonPoints.Length == 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                Transform point = summonPoints[i % summonPoints.Length];
                EnemyActor add = Instantiate(prefab, point.position, point.rotation);
                add.Defeated += HandleAddDefeated;
                liveAdds.Add(add);
            }
        }

        private void HandleAddDefeated(EnemyActor add)
        {
            add.Defeated -= HandleAddDefeated;
            liveAdds.Remove(add);
            PublishVitals();
        }

        private void PublishVitals()
        {
            BossVitalsChanged?.Invoke(health, maxHealth, aegis, maxAegis, IsVulnerable);
        }
    }
}
