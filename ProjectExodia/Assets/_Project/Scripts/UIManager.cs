using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class UIManager : ManagerBase
    {
        private readonly Dictionary<Type, MenuPanel> _menuPanels = new();
        private readonly List<MenuPanel> _shownPanels = new();

        public void ShowPanel<T>(bool closeOtherPanels = true) where T : MenuPanel
        {
            var classType = typeof(T);
            if (!_menuPanels.ContainsKey(classType))
            {
                Debug.LogWarning("Failed to show non-existent panel.");
                return;
            }

            var panel = _menuPanels[classType];
            panel.ShowPanel();

            if (closeOtherPanels)
            {
                foreach (var otherPanel in _shownPanels)
                    otherPanel.HidePanel();
                
                _shownPanels.Clear();
            }
            
            _shownPanels.Add(panel);
        }

        public void HidePanel<T>() where T : MenuPanel
        {
            var classType = typeof(T);
            if (!_menuPanels.ContainsKey(classType))
            {
                Debug.LogWarning("Failed to hide non-existent panel.");
                return;
            }
            
            var panel = _menuPanels[classType];
            panel.HidePanel();
            _shownPanels.Remove(panel);
        }
    }
}
