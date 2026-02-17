using System.Collections.Generic;
using UnityEngine;

public class SharedCameraController : MonoBehaviour
{
    [SerializeField] private List<Transform> trackedPlayers = new List<Transform>();
    [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -8f);
    [SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (trackedPlayers.Count == 0)
        {
            return;
        }

        Vector3 center = Vector3.zero;
        int validCount = 0;

        foreach (Transform player in trackedPlayers)
        {
            if (player == null)
            {
                continue;
            }

            center += player.position;
            validCount++;
        }

        if (validCount == 0)
        {
            return;
        }

        center /= validCount;
        Vector3 targetPosition = center + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);
        transform.LookAt(center);
    }

    public void RegisterPlayer(Transform player)
    {
        if (!trackedPlayers.Contains(player))
        {
            trackedPlayers.Add(player);
        }
    }
}
