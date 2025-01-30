using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SpellsBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellNameText;
    [SerializeField] private Timer timer;
    [SerializeField] private List<MonoBehaviour> spellBehaviours = new List<MonoBehaviour>();
    [SerializeField] private Image spellIconImage; // UI Image for the spell icon
    [SerializeField] private Sprite defaultIcon; // Default icon for no spell or unknown spell
    [SerializeField] private List<SpellIconMapping> spellIconMappings; // List to map spell names to icons

    private ISpellBehaviour currentSpell;
    private bool isCooldownActive = false;
    private string queuedSpell = null;
    private int lastTimerSecond = -1;

    private Dictionary<string, Sprite> spellIcons; // Internal dictionary for quick icon lookup

    [System.Serializable]
    public class SpellIconMapping
    {
        public string spellName; // Name of the spell
        public Sprite icon;      // Associated icon sprite
    }

    void Start()
    {

        InitializeSpellIcons();

        List<MonoBehaviour> validSpells = new List<MonoBehaviour>();

        foreach (var spell in spellBehaviours)
        {
            if (spell is ISpellBehaviour)
            {
                validSpells.Add(spell);
            }
        }

        spellBehaviours = validSpells;

        ChooseRandomSpell();
        UpdateSpellDisplay();
    }

    void Update()
    {
        CheckCurrentSpellHitMob();

        int currentSecond = timer.GetSecondsCount();

        // Check if the timer is at a multiple of 10 and hasn't already been updated for this interval
        if (currentSecond % 10 == 0 && currentSecond != lastTimerSecond)
        {
            ChooseDifferentCurrentSpell();
            lastTimerSecond = currentSecond; // Update the last updated second
        }
    }

    public void ChooseRandomSpell()
    {
        int randomIndex = Random.Range(0, spellBehaviours.Count);
        SetCurrentSpell(spellBehaviours[randomIndex] as ISpellBehaviour);
    }

    public void ChooseDifferentCurrentSpell()
    {
        ISpellBehaviour newSpell;
        do
        {
            newSpell = spellBehaviours[Random.Range(0, spellBehaviours.Count)] as ISpellBehaviour;
        } while (newSpell == currentSpell);

        SetCurrentSpell(newSpell);
    }

    public void SetCurrentSpell(ISpellBehaviour newSpell)
    {
        if (isCooldownActive)
        {
            Debug.Log($"Cooldown active. Queuing spell: {newSpell.GetType().Name}");
            queuedSpell = newSpell.GetType().Name;
            return;
        }

        currentSpell = newSpell;
        UpdateSpellDisplay();

        if (newSpell.GetType().Name == "BlizzardBehaviour" ||
            newSpell.GetType().Name == "TempestBehaviour" ||
            newSpell.GetType().Name == "EarthballBehaviour")
        {
            InvokeCurrentSpellOnce(); // Automatically invoke specific spells
        }
    }

    public void InvokeCurrentSpellOnce()
    {
        if (isCooldownActive) return;

        currentSpell?.InvokeSpell();
        StartCooldown();
    }

    private void StartCooldown()
    {
        isCooldownActive = true;
        Invoke(nameof(EndCooldown), 10f);
    }

    private void EndCooldown()
    {
        isCooldownActive = false;

        if (!string.IsNullOrEmpty(queuedSpell))
        {
            Debug.Log($"Cooldown ended. Invoking queued spell: {queuedSpell}");
            var queuedSpellBehaviour = spellBehaviours.Find(spell => spell.GetType().Name == queuedSpell);
            SetCurrentSpell(queuedSpellBehaviour as ISpellBehaviour);
            queuedSpell = null;
        }
    }

    public void CheckCurrentSpellHitMob()
    {
        currentSpell?.CheckIfHitMob();
    }

    public string GetCurrentSpell()
    {
        return currentSpell?.GetType().Name ?? "None";
    }

    private void UpdateSpellDisplay()
    {
        UpdateSpellName();
        UpdateSpellIcon();
    }

    private void UpdateSpellName()
    {
        if (spellNameText != null && currentSpell != null)
        {
            string displayName = currentSpell.GetType().Name.Replace("Behaviour", "");
            spellNameText.text = displayName;
        }
        else if (spellNameText != null)
        {
            spellNameText.text = "No Spell Selected";
        }
    }

    private void UpdateSpellIcon()
    {

        if (spellIconImage == null)
        {
            Debug.LogWarning("Spell Icon Image is not assigned in the Inspector.");
            return;
        }

        if (spellIcons == null || spellIcons.Count == 0)
        {
            Debug.LogWarning("Spell Icons dictionary is not initialized or empty.");
            spellIconImage.sprite = defaultIcon; // Set to default icon as fallback
            return;
        }

        string spellName = currentSpell?.GetType().Name;
        Debug.Log($"Looking for spell icon: {spellName}");
        if (spellName != null && spellIcons.ContainsKey(spellName))
        {
            spellIconImage.sprite = spellIcons[spellName];
        }
        else
        {
            spellIconImage.sprite = defaultIcon; // Default icon for unknown spells
        }
    }

    void InitializeSpellIcons()
    {
        spellIcons = new Dictionary<string, Sprite>();

        foreach (var mapping in spellIconMappings)
        {
            if (!string.IsNullOrEmpty(mapping.spellName) && mapping.icon != null)
            {
                spellIcons[mapping.spellName] = mapping.icon;
                Debug.Log($"Mapped {mapping.spellName} to an icon.");
            }
            else
            {
                Debug.LogWarning("A mapping entry is invalid: Spell name or icon is null.");
            }
        }

        if (spellIcons.Count == 0)
        {
            Debug.LogWarning("No valid spell mappings found. Spell Icons dictionary is empty.");
        }
    }


    public void CastCurrentSpell()
    {
        currentSpell?.InvokeSpell();
    }
}
