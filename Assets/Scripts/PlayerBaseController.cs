using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerBaseController : MonoBehaviour
{
    public enum PlayerRole
    {
        Pistols,
        Tank,
        Archer,
        Mage
    }

    [SerializeField] private PlayerRole role;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float ability1Cooldown = 4f;
    [SerializeField] private float ability2Cooldown = 8f;
    [SerializeField] private MageCommandSystem mageCommandSystem;

    private CharacterController characterController;
    private Vector3 velocity;
    private float ability1Timer;
    private float ability2Timer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        ability1Timer -= Time.deltaTime;
        ability2Timer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryAbility1();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryAbility2();
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0f, vertical);
        characterController.Move(move * moveSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (characterController.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
    }

    private void TryAbility1()
    {
        if (ability1Timer > 0f)
        {
            return;
        }

        switch (role)
        {
            case PlayerRole.Pistols:
                Debug.Log("Pistols: Quick Burst");
                break;
            case PlayerRole.Tank:
                Debug.Log("Tank: Shield Slam");
                break;
            case PlayerRole.Archer:
                Debug.Log("Archer: Piercing Shot");
                break;
            case PlayerRole.Mage:
                if (GameManager.Instance.TeamResourceManager.TrySpend(10f))
                {
                    Debug.Log("Mage: Arc Sigil Blast");
                }
                break;
        }

        ability1Timer = ability1Cooldown;
    }

    private void TryAbility2()
    {
        if (ability2Timer > 0f)
        {
            return;
        }

        switch (role)
        {
            case PlayerRole.Pistols:
                Debug.Log("Pistols: Roll Reload");
                break;
            case PlayerRole.Tank:
                Debug.Log("Tank: Taunt Aura");
                break;
            case PlayerRole.Archer:
                Debug.Log("Archer: Evasive Volley");
                break;
            case PlayerRole.Mage:
                if (mageCommandSystem != null && GameManager.Instance.TeamResourceManager.TrySpend(15f))
                {
                    mageCommandSystem.MarkTargetFromScreenCenter();
                }
                break;
        }

        ability2Timer = ability2Cooldown;
    }
}
