using UnityEngine;

namespace CodexGame.Enemies
{
    public class ThrallEnemy : EnemyActor
    {
        [SerializeField] private float moveSpeed = 4f;
        [SerializeField] private float contactDamage = 8f;

        public float MoveSpeed => moveSpeed;
        public float ContactDamage => contactDamage;
    }
}
