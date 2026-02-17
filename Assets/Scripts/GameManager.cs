using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TeamResourceManager teamResourceManager;
    [SerializeField] private MageImpManager mageImpManager;
    [SerializeField] private EncounterController encounterController;

    public TeamResourceManager TeamResourceManager => teamResourceManager;
    public MageImpManager MageImpManager => mageImpManager;
    public EncounterController EncounterController => encounterController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
