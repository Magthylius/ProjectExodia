using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    [System.Serializable]
    public struct FSpawnVariables
    {
        [Tooltip("distanceStamp: Gap between each enemy in Z Axis")]
        public float distanceStamp;
        
        [Tooltip("spawnIndex: -5 = left, 0 = middle, 5 = right")]
        public int spawnIndex;
    }

    [CreateAssetMenu(fileName = "SpawnPattern", menuName = "ScriptableObjects/SpawnManagerScriptableObject")]
    public class SpawnPattern : ScriptableObject
    {
        
        [Header("Parameters")]
        public List<FSpawnVariables> patternList = new();
        public float spawnDistanceBetweenEachPattern = 100.0f;
    }
}

