using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerInput inputs;
    private InputAction moveaction;
    private Animator anim;
    private InputAction fireaction;
    private GameManager manager;
    [SerializeField] private SpellsBehaviour spells;
    private Vector2 velocity = Vector2.zero;
    private int direction = 0;
    [SerializeField] private float speed = 5f;
    private GameObject currentEarthBall;
    [SerializeField] private Timer timer;

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
        if (moveaction == null) return; // Sécurité si l'action est null.
        Vector2 _moveValue = moveaction.ReadValue<Vector2>();
        _moveValue = ChooseDirection(_moveValue);
        velocity = _moveValue * speed;

        transform.position += new Vector3(velocity.x * Time.fixedDeltaTime, velocity.y * Time.fixedDeltaTime, 0);

        //Animation
        anim.SetInteger("directions", direction);
    }

    private void Update()
    {
        if (fireaction.triggered)
        {
            spells.InvokeCurrentSpell();
            spells.GetCurrentSpell();
        }

        // Disable mouse click for 10s when Blizzard or Tempest is active
        if (spells.GetCurrentSpell() == "BlizzardBehaviour" || spells.GetCurrentSpell() == "TempestBehaviour")
        {
            //spells.InvokeCurrentSpellOnce();
            fireaction.Disable();
            Invoke("EnableFireAction", 10f);
        } // FIX THE LOGIC HERE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    }

    private Vector2 ChooseDirection(Vector2 _value)
    {
        Vector2 _result = Vector2.zero;

        // Choix de la direction principale selon l'axe ayant la valeur la plus grande
        if (Mathf.Abs(_value.x) >= Mathf.Abs(_value.y))
        {
            _result = new Vector2(_value.x, 0); // Priorité sur X
        }
        else
        {
            _result = new Vector2(0, _value.y); // Priorité sur Y
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

    public void ResetSpeed()
    {
        speed = 5f;
    }

    private void EnableFireAction()
    {
        fireaction.Enable();
    }
}
