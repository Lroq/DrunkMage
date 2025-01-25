using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerInput inputs;
    private InputAction moveaction;
    private Animator anim;
    private InputAction fireaction;
    private InputAction dashaction;
    private GameManager manager;
    [SerializeField] private SpellsBehaviour spells;
    private Vector2 velocity = Vector2.zero;
    private int direction = 0;
    [SerializeField] private float speed = 5f;
    private GameObject currentEarthBall;

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    void Start()
    {
        manager = GameManager.GetInstance();
        inputs = manager.GetInputs();
        moveaction = inputs.actions.FindAction("Move");
        fireaction = inputs.actions.FindAction("Fire");
        dashaction = inputs.actions.FindAction("Dash");

        GameObject spellsManager = GameObject.Find("Spells");
        if (spellsManager != null)
        {
            spells = spellsManager.GetComponent<SpellsBehaviour>();
        }

        if (spells == null)
        {
            Debug.LogWarning("Spells manager is not assigned.");
        }

        spells.ChooseRandomSpell();
    }

    private void FixedUpdate()
    {
        if (moveaction == null) return;

        Vector2 _moveValue = moveaction.ReadValue<Vector2>();
        _moveValue = ChooseDirection(_moveValue);
        velocity = _moveValue * speed;

        transform.position += new Vector3(velocity.x * Time.fixedDeltaTime, velocity.y * Time.fixedDeltaTime, 0);

        
    }

    private void Update()
    {
        if (spells.GetCurrentSpell() == "DrunkBoomBehaviour" || spells.GetCurrentSpell() == "FireballBehaviour" || spells.GetCurrentSpell() == "DivineSmiteBehaviour")
        {
            if (fireaction.triggered)
            {
                Debug.Log("Fire!");
                spells.CastCurrentSpell();
            }
        }

        // Trigger dash ability if the player presses the dash button
        if (dashaction.triggered)
        {
            Debug.Log("Dash!");
            transform.position += new Vector3(velocity.x * 2 * Time.deltaTime, velocity.y * 2 * Time.deltaTime, 0);
        }
    }

    private Vector2 ChooseDirection(Vector2 _value)
    {
        Vector2 _result = Vector2.zero;

        if (Mathf.Abs(_value.x) >= Mathf.Abs(_value.y))
        {
            _result = new Vector2(_value.x, 0); // Prioritize X
        }
        else
        {
            _result = new Vector2(0, _value.y); // Prioritize Y
        }
        direction = SetDirection(_result);
        return _result;
    }

    private int SetDirection(Vector2 _vector)
    {
        if (_vector.x > 0) return 6;
        if (_vector.x < 0) return 4;
        if (_vector.y > 0) return 8;
        if (_vector.y < 0) return 2;
        return 0;
    }
}
