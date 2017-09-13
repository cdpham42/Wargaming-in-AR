// Shared Space Project 2
// Booz Allen Hamilton, Seattle Summer Games Team
// Tremaine Ng
// July 31st, 2017
// Holds list of spawnable prefabs by a string name, and allows access by string name
// be sure to include these prefabs in the NetworkManager if using online functionality
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableList : MonoBehaviour {

    // list of prefabs to spawn; add more fields as needed
    [Serializable]
    public struct NamedSpawnable
    {
        public string Name;
        public GameObject Prefab;
    }
    public NamedSpawnable[] Spawnables;

    public Dictionary<string, GameObject> UnitDictionary;

    private void Awake()
    {
        UnitDictionary = new Dictionary<string, GameObject>();
        foreach (NamedSpawnable spawnable in Spawnables)
        {
            UnitDictionary.Add(spawnable.Name, spawnable.Prefab);
        }
    }
}
