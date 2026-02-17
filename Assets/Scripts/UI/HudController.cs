using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodexGame.UI
{
    public class HudController : MonoBehaviour
    {
        [Header("Team Sigils")]
        [SerializeField] private Image[] teamSigils = new Image[4];
        [SerializeField] private Color activeSigilColor = Color.white;
        [SerializeField] private Color inactiveSigilColor = new(1f, 1f, 1f, 0.2f);

        [Header("Imp Counter")]
        [SerializeField] private TMP_Text impCountLabel;

        [Header("Boss HP")]
        [SerializeField] private Slider bossHpSlider;
        [SerializeField] private TMP_Text bossHpLabel;

        public void SetConnectedPlayers(int playerCount)
        {
            int clamped = Mathf.Clamp(playerCount, 0, teamSigils.Length);
            for (int i = 0; i < teamSigils.Length; i++)
            {
                if (teamSigils[i] != null)
                {
                    teamSigils[i].color = i < clamped ? activeSigilColor : inactiveSigilColor;
                }
            }
        }

        public void SetImpCount(int count)
        {
            int clamped = Mathf.Clamp(count, 0, 8);
            if (impCountLabel != null)
            {
                impCountLabel.text = $"Imps: {clamped}/8";
            }
        }

        public void SetBossHp(float currentHp, float maxHp)
        {
            float safeMax = Mathf.Max(1f, maxHp);
            float clampedCurrent = Mathf.Clamp(currentHp, 0f, safeMax);

            if (bossHpSlider != null)
            {
                bossHpSlider.value = clampedCurrent / safeMax;
            }

            if (bossHpLabel != null)
            {
                bossHpLabel.text = $"Boss HP {Mathf.CeilToInt(clampedCurrent)}/{Mathf.CeilToInt(safeMax)}";
            }
        }
    }
}
