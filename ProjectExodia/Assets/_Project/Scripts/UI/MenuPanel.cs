using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class MenuPanel : MonoBehaviour
    {
        protected UIManager UIManager;
        
        public virtual void Initialize(UIManager uiManager)
        {
            UIManager = uiManager;
        }
        
        public virtual void ShowPanel()
        {
            gameObject.SetActive(true);
        }

        public virtual void HidePanel()
        {
            gameObject.SetActive(false);
        }
    }
}
