using System;
using UnityEngine;

namespace CodexGame.Enemies
{
    public abstract class EnemyActor : MonoBehaviour
    {
        [Header("Base Stats")]
        [SerializeField] private string displayName = "Enemy";
        [SerializeField] private float maxHealth = 30f;

        public string DisplayName => displayName;
        public float MaxHealth => maxHealth;
        public float CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0f;

        public event Action<EnemyActor> Defeated;

        protected virtual void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public virtual void TakeDamage(float amount)
        {
            if (IsDead || amount <= 0f)
            {
                return;
            }

            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            if (CurrentHealth <= 0f)
            {
                OnDefeated();
            }
        }

        protected virtual void OnDefeated()
        {
            Defeated?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
