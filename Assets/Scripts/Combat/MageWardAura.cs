using System;
using UnityEngine;

namespace CodexGame.Combat
{
    public class MageWardAura : MonoBehaviour
    {
        [SerializeField] private TankHealth tank;
        [SerializeField] private float radius = 8f;
        [SerializeField] private float healPerSecond = 20f;
        [SerializeField] private LayerMask tankLayerMask = ~0;
        [SerializeField] private bool drawGizmo = true;

        public event Action<bool> TankHealingStateChanged;

        public bool IsTankInsideAura { get; private set; }

        private readonly Collider[] overlapHits = new Collider[8];

        private void Reset()
        {
            if (tank == null)
            {
                tank = FindFirstObjectByType<TankHealth>();
            }
        }

        private void Update()
        {
            if (tank == null || !tank.IsAlive)
            {
                SetTankInsideAura(false);
                return;
            }

            bool shouldHeal = IsTankWithinRadius();
            SetTankInsideAura(shouldHeal);

            if (shouldHeal)
            {
                tank.Heal(healPerSecond * Time.deltaTime);
            }
        }

        private bool IsTankWithinRadius()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(transform.position, radius, overlapHits, tankLayerMask);
            for (int i = 0; i < hitCount; i++)
            {
                if (overlapHits[i] == null)
                {
                    continue;
                }

                if (overlapHits[i].GetComponentInParent<TankHealth>() == tank)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetTankInsideAura(bool value)
        {
            if (IsTankInsideAura == value)
            {
                return;
            }

            IsTankInsideAura = value;
            TankHealingStateChanged?.Invoke(IsTankInsideAura);
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmo)
            {
                return;
            }

            Gizmos.color = IsTankInsideAura ? Color.green : Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
