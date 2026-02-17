using UnityEngine;

public class MageWardAura : MonoBehaviour
{
    [SerializeField] private float wardRadius = 6f;
    [SerializeField] private float wardTickSeconds = 1f;
    [SerializeField] private float wardHealPerTick = 3f;
    [SerializeField] private LayerMask allyMask;

    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0f)
        {
            return;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, wardRadius, allyMask);
        foreach (Collider col in colliders)
        {
            Health health = col.GetComponent<Health>();
            health?.Heal(wardHealPerTick);
        }

        timer = wardTickSeconds;
    }
}
