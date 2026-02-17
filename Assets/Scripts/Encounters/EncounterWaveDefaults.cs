using CodexGame.Enemies;

namespace CodexGame.Encounters
{
    public static class EncounterWaveDefaults
    {
        public static EncounterController.Wave[] GetDefault(EncounterRoomId room)
        {
            return room switch
            {
                EncounterRoomId.Courtyard => new[]
                {
                    BuildWave(0f, (EnemyType.Thrall, 6)),
                    BuildWave(2f, (EnemyType.StoneHusk, 1), (EnemyType.Thrall, 4))
                },
                EncounterRoomId.RunePath => new[]
                {
                    BuildWave(0f, (EnemyType.HexArcher, 3), (EnemyType.Thrall, 3)),
                    BuildWave(3f, (EnemyType.StoneHusk, 2))
                },
                EncounterRoomId.Bridge => new[]
                {
                    BuildWave(0f, (EnemyType.Thrall, 8)),
                    BuildWave(4f, (EnemyType.HexArcher, 2), (EnemyType.StoneHusk, 1))
                },
                EncounterRoomId.LeverLeft => new[]
                {
                    BuildWave(0f, (EnemyType.Thrall, 4), (EnemyType.HexArcher, 1))
                },
                EncounterRoomId.LeverRight => new[]
                {
                    BuildWave(0f, (EnemyType.Thrall, 4), (EnemyType.StoneHusk, 1))
                },
                _ => new[] { BuildWave(0f, (EnemyType.Thrall, 3)) }
            };
        }

        private static EncounterController.Wave BuildWave(float delay, params (EnemyType type, int count)[] entries)
        {
            var wave = new EncounterController.Wave
            {
                delayBeforeSpawn = delay,
                spawns = new EncounterController.SpawnInstruction[entries.Length]
            };

            for (int i = 0; i < entries.Length; i++)
            {
                wave.spawns[i] = new EncounterController.SpawnInstruction
                {
                    enemyType = entries[i].type,
                    count = entries[i].count
                };
            }

            return wave;
        }
    }
}
