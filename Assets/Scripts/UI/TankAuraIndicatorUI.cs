using CodexGame.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace CodexGame.UI
{
    public class TankAuraIndicatorUI : MonoBehaviour
    {
        [SerializeField] private MageWardAura mageWardAura;
        [SerializeField] private Graphic indicatorGraphic;
        [SerializeField] private Color inactiveColor = new(1f, 1f, 1f, 0.35f);
        [SerializeField] private Color activeColor = new(0.35f, 1f, 0.35f, 1f);

        private void Awake()
        {
            if (mageWardAura == null)
            {
                mageWardAura = FindFirstObjectByType<MageWardAura>();
            }

            RefreshVisual(mageWardAura != null && mageWardAura.IsTankInsideAura);
        }

        private void OnEnable()
        {
            if (mageWardAura != null)
            {
                mageWardAura.TankHealingStateChanged += RefreshVisual;
            }
        }

        private void OnDisable()
        {
            if (mageWardAura != null)
            {
                mageWardAura.TankHealingStateChanged -= RefreshVisual;
            }
        }

        private void RefreshVisual(bool isActive)
        {
            if (indicatorGraphic != null)
            {
                indicatorGraphic.color = isActive ? activeColor : inactiveColor;
            }
        }
    }
}
