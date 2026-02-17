using UnityEngine;

public enum TeamAlignment
{
    Players,
    Enemies
}

public class DamageSource : MonoBehaviour
{
    [SerializeField] private TeamAlignment team;
    [SerializeField] private float baseDamage = 10f;

    public TeamAlignment Team => team;

    public void DealDamage(Health target)
    {
        if (target == null)
        {
            return;
        }

        target.ApplyDamage(baseDamage, gameObject);

        if (team == TeamAlignment.Players && GameManager.Instance != null)
        {
            GameManager.Instance.TeamResourceManager.RegisterTeamDamage(baseDamage);
        }
    }
}
