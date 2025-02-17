using UnityEngine;
using System.Collections.Generic;

public class DivineSmiteBehaviour : MonoBehaviour, ISpellBehaviour
{
    [SerializeField] private GameObject _divineSmitePrefab;
    [SerializeField] private MobSpawner mob;

    private List<GameObject> _spawnedMobs;
    private GameObject _currentDivineSmite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeSpell()
    {
        _spawnedMobs = mob.GetListOfMobsGenerated();
        if (_divineSmitePrefab == null)
        {
            Debug.LogWarning("Divine Smite prefab is not assigned.");
            return;
        }

        // Check if there are any mobs available
        if (_spawnedMobs == null || _spawnedMobs.Count == 0)
        {
            Debug.LogWarning("No mobs available to smite.");
            return;
        }

        // Choose a random mob from the list
        int randomIndex = Random.Range(0, _spawnedMobs.Count);
        GameObject selectedMob = _spawnedMobs[randomIndex];

        if (selectedMob == null)
        {
            Debug.LogWarning("Selected mob has already been destroyed.");
            return;
        }

        // Spawn the divine smite at the selected mob's position if the mob is within the camera view
        if (selectedMob.transform.position.x < Camera.main.transform.position.x + 10)
        {
            _currentDivineSmite = Instantiate(_divineSmitePrefab, selectedMob.transform.position, Quaternion.identity);

        }
        Destroy(selectedMob);

        // Remove the destroyed mob from the list
        _spawnedMobs.RemoveAt(randomIndex);

        // Destroy the divine smite after a set amount of time
        Destroy(_currentDivineSmite, 0.2f);
    }

    public void MoveSpell(GameObject spell)
    {
    }

    public void CheckIfHitMob()
    {
    }

}