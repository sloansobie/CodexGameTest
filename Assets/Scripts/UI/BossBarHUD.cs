using CodexGame.Boss;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodexGame.UI
{
    public class BossBarHUD : MonoBehaviour
    {
        [SerializeField] private BossController bossController;
        [SerializeField] private CanvasGroup root;
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider aegisBar;
        [SerializeField] private TMP_Text stateText;

        private void OnEnable()
        {
            if (bossController == null)
            {
                return;
            }

            bossController.BossBarVisibilityChanged += SetVisible;
            bossController.BossVitalsChanged += HandleVitalsChanged;
        }

        private void OnDisable()
        {
            if (bossController == null)
            {
                return;
            }

            bossController.BossBarVisibilityChanged -= SetVisible;
            bossController.BossVitalsChanged -= HandleVitalsChanged;
        }

        private void SetVisible(bool visible)
        {
            root.alpha = visible ? 1f : 0f;
            root.interactable = visible;
            root.blocksRaycasts = visible;
        }

        private void HandleVitalsChanged(float health, float maxHealth, float aegis, float maxAegis, bool isVulnerable)
        {
            healthBar.value = maxHealth <= 0f ? 0f : health / maxHealth;
            aegisBar.value = maxAegis <= 0f ? 0f : aegis / maxAegis;
            stateText.text = isVulnerable ? "Vulnerable" : "Shielded by adds";
        }
    }
}
