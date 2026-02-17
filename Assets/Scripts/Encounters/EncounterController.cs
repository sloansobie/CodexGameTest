using System;
using System.Collections.Generic;
using CodexGame.Enemies;
using UnityEngine;

namespace CodexGame.Encounters
{
    public class EncounterController : MonoBehaviour
    {
        [Serializable]
        public class SpawnInstruction
        {
            public EnemyType enemyType;
            public int count = 1;
        }

        [Serializable]
        public class Wave
        {
            public float delayBeforeSpawn;
            public SpawnInstruction[] spawns;
        }

        [Serializable]
        public class RoomConfig
        {
            public EncounterRoomId roomId;
            public Wave[] waves;
        }

        [Header("Trigger + Boundaries")]
        [SerializeField] private Collider triggerVolume;
        [SerializeField] private Collider[] roomBlockers;

        [Header("Spawn")]
        [SerializeField] private EnemyPrefabRegistry enemyPrefabs;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private RoomConfig[] roomConfigs;
        [SerializeField] private EncounterRoomId roomId;

        public event Action<EncounterController> EncounterCompleted;

        private readonly List<EnemyActor> liveEnemies = new();
        private bool started;

        private void Awake()
        {
            SetBlockersLocked(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (started || !other.CompareTag("Player"))
            {
                return;
            }

            if (triggerVolume != null && !triggerVolume.bounds.Intersects(other.bounds))
            {
                return;
            }

            StartEncounter();
        }

        public void StartEncounter()
        {
            if (started)
            {
                return;
            }

            started = true;
            SetBlockersLocked(true);
            Wave[] waves = ResolveWaves(roomId);
            StartCoroutine(RunWaves(waves));
        }

        private System.Collections.IEnumerator RunWaves(Wave[] waves)
        {
            foreach (Wave wave in waves)
            {
                if (wave.delayBeforeSpawn > 0f)
                {
                    yield return new WaitForSeconds(wave.delayBeforeSpawn);
                }

                SpawnWave(wave);
                while (liveEnemies.Count > 0)
                {
                    yield return null;
                }
            }

            SetBlockersLocked(false);
            EncounterCompleted?.Invoke(this);
        }

        private void SpawnWave(Wave wave)
        {
            if (wave.spawns == null)
            {
                return;
            }

            for (int i = 0; i < wave.spawns.Length; i++)
            {
                SpawnInstruction instruction = wave.spawns[i];
                EnemyActor prefab = enemyPrefabs.GetPrefab(instruction.enemyType);
                if (prefab == null)
                {
                    continue;
                }

                for (int count = 0; count < instruction.count; count++)
                {
                    Transform point = spawnPoints[(i + count) % spawnPoints.Length];
                    EnemyActor enemy = Instantiate(prefab, point.position, point.rotation);
                    liveEnemies.Add(enemy);
                    enemy.Defeated += HandleEnemyDefeated;
                }
            }
        }

        private void HandleEnemyDefeated(EnemyActor enemy)
        {
            enemy.Defeated -= HandleEnemyDefeated;
            liveEnemies.Remove(enemy);
        }

        private Wave[] ResolveWaves(EncounterRoomId id)
        {
            foreach (RoomConfig config in roomConfigs)
            {
                if (config.roomId == id)
                {
                    return config.waves;
                }
            }

            return EncounterWaveDefaults.GetDefault(id);
        }

        private void SetBlockersLocked(bool isLocked)
        {
            foreach (Collider blocker in roomBlockers)
            {
                if (blocker != null)
                {
                    blocker.enabled = isLocked;
                }
            }
        }
    }

    public enum EncounterRoomId
    {
        Courtyard,
        RunePath,
        Bridge,
        LeverLeft,
        LeverRight
    }
}
