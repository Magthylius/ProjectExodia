using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class EffectsPanel : MenuPanel
    {
        [SerializeField] private GameObject goBananaFireObject;

        private void Awake()
        {
            goBananaFireObject.SetActive(false);
        }

        public void SetGoBananaFire(bool active) => goBananaFireObject.SetActive(true);
    }
}
