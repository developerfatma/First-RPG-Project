using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentGameObjectPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObject();

            hasSpawned = true;


        }

        void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistentGameObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}

