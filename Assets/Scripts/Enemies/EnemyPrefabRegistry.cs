using System;
using UnityEngine;

namespace CodexGame.Enemies
{
    [CreateAssetMenu(menuName = "Codex/Enemies/Enemy Prefab Registry")]
    public class EnemyPrefabRegistry : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public EnemyType enemyType;
            public EnemyActor prefab;
        }

        [SerializeField] private Entry[] entries;

        public EnemyActor GetPrefab(EnemyType type)
        {
            foreach (Entry entry in entries)
            {
                if (entry.enemyType == type)
                {
                    return entry.prefab;
                }
            }

            return null;
        }
    }

    public enum EnemyType
    {
        Thrall,
        HexArcher,
        StoneHusk
    }
}
