using UnityEngine;

namespace CodexGame.Enemies
{
    public class HexArcherEnemy : EnemyActor
    {
        [SerializeField] private float projectileDamage = 12f;
        [SerializeField] private float attackInterval = 1.6f;
        [SerializeField] private float engagementRange = 9f;

        public float ProjectileDamage => projectileDamage;
        public float AttackInterval => attackInterval;
        public float EngagementRange => engagementRange;
    }
}
