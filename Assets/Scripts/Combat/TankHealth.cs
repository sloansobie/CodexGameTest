using UnityEngine;

namespace CodexGame.Combat
{
    public class TankHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 300f;
        [SerializeField] private float currentHealth = 300f;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;
        public bool IsAlive => currentHealth > 0f;

        private void Awake()
        {
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }

        public void Heal(float amount)
        {
            if (!IsAlive || amount <= 0f)
            {
                return;
            }

            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        }

        public void TakeDamage(float amount)
        {
            if (!IsAlive || amount <= 0f)
            {
                return;
            }

            currentHealth = Mathf.Max(0f, currentHealth - amount);
        }
    }
}
