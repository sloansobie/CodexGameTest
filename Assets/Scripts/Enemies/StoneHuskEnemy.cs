using UnityEngine;

namespace CodexGame.Enemies
{
    public class StoneHuskEnemy : EnemyActor
    {
        [SerializeField] private float armor = 0.35f;
        [SerializeField] private float slamDamage = 16f;

        public float Armor => armor;
        public float SlamDamage => slamDamage;

        public override void TakeDamage(float amount)
        {
            float mitigated = Mathf.Max(1f, amount * (1f - armor));
            base.TakeDamage(mitigated);
        }
    }
}
