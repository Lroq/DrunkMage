using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SpellsBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellNameText;
    [SerializeField] private Timer timer;
    [SerializeField] private List<MonoBehaviour> spellBehaviours = new List<MonoBehaviour>();
    private ISpellBehaviour currentSpell;
    private bool isCooldownActive = false;
    private string queuedSpell = null;
    private int lastTimerSecond = -1;

    void Start()
    {
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
        UpdateSpellName();
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
        UpdateSpellName();

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

    private void UpdateSpellName()
    {
        if (spellNameText != null && currentSpell != null)
        {
            string displayName = currentSpell.GetType().Name.Replace("Behaviour", "");
            spellNameText.text = "Current Spell: " + displayName;
        }
        else if (spellNameText != null)
        {
            spellNameText.text = "No Spell Selected";
        }
    }

    public void CastCurrentSpell()
    {
        currentSpell?.InvokeSpell();
    }
}
