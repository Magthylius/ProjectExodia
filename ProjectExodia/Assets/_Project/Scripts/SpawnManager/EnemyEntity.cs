using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class EnemyEntity : MonoBehaviour, ISlappableEntity
    {
        public void PerformSlap()
        {
            
        }

        public string GetName() => gameObject.name;
    }
}
