using UnityEngine;

public interface ISpellBehaviour
{
    void InvokeSpell();
    void CheckIfHitMob();
    void MoveSpell(GameObject spell);
}
