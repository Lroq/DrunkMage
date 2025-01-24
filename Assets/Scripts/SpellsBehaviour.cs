using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class SpellsBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spellNameText;
    [SerializeField] private Timer timer;
    [SerializeField] private List<MonoBehaviour> spellBehaviours = new List<MonoBehaviour>();
    private ISpellBehaviour currentSpell;
    private int lastTimerSecond = -1; // Track the last timer second when the spell was changed

    void Start()
    {
        // Create a new list for valid spell behaviors
        List<MonoBehaviour> validSpells = new List<MonoBehaviour>();

        foreach (var spell in spellBehaviours)
        {
            if (spell is ISpellBehaviour)
            {
                validSpells.Add(spell); // Add valid spells to the new list
            }
        }

        // Replace the original list with the filtered one
        spellBehaviours = validSpells;

        // Choose a random spell as the current spell
        ChooseRandomSpell();
        UpdateSpellName();
    }

    void Update()
    {
        // Get the current second count from the timer
        int currentSecond = timer.GetSecondsCount();

        // Change the spell only if the timer is at a multiple of 10 and hasn't already been updated for this interval
        if (currentSecond % 10 == 0 && currentSecond != lastTimerSecond)
        {
            ChooseDifferentCurrentSpell();
            lastTimerSecond = currentSecond; // Update the last updated time
        }

        CheckCurrentSpellHitMob();
    }

    public void ChooseRandomSpell()
    {
        int randomIndex = Random.Range(0, spellBehaviours.Count);
        currentSpell = spellBehaviours[randomIndex] as ISpellBehaviour;
    }

    public void ChooseDifferentCurrentSpell()
    {
        ISpellBehaviour newSpell;
        do
        {
            newSpell = spellBehaviours[Random.Range(0, spellBehaviours.Count)] as ISpellBehaviour;
        } while (newSpell == currentSpell);

        currentSpell = newSpell;

        UpdateSpellName();
    }

    public void InvokeCurrentSpell()
    {
        currentSpell?.InvokeSpell();
    }

    public void MoveCurrentSpell(GameObject spell)
    {
        currentSpell?.MoveSpell(spell);
    }

    public void CheckCurrentSpellHitMob()
    {
        currentSpell?.CheckIfHitMob();
    }

    public string GetCurrentSpell()
    {
        return currentSpell.GetType().Name;
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

   /* // Invoke current spell once and reactive invoke current spell after 10 seconds
    public void InvokeCurrentSpellOnce()
    {
        InvokeCurrentSpell();
        Invoke("InvokeCurrentSpellOnce", 10f);
    }*/
}
