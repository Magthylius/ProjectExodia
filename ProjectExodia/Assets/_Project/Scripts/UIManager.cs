using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class UIManager : ManagerBase
    {
        [SerializeField] private MainMenuPanel mainMenuPrefab;
        [SerializeField] private MainHUDPanel mainHUDPrefab;
        [SerializeField] private DebugPanel debugPanelPrefab;
        [SerializeField] private EffectsPanel effectsPanelPrefab;
        [SerializeField] private EndPanel endPanelPrefab;
        [SerializeField] private TransitionPanel transitionPanelPrefab;

        private readonly Dictionary<Type, MenuPanel> _menuPanels = new();
        private readonly List<MenuPanel> _shownPanels = new();

        private void Start()
        {
            CreatePanel(debugPanelPrefab, true);
            CreatePanel(effectsPanelPrefab, true);
            CreatePanel(mainHUDPrefab);
            CreatePanel(mainMenuPrefab, true);
            CreatePanel(endPanelPrefab);
            CreatePanel(transitionPanelPrefab);
        }

        public void ResetPanels()
        {
            ShowPanel<DebugPanel>(false);
            ShowPanel<EffectsPanel>(false);
            ShowPanel<MainMenuPanel>(false);

            HidePanel<MainHUDPanel>();
            HidePanel<EndPanel>();
            HidePanel<TransitionPanel>();
        }

        private void CreatePanel<T>(T panelPrefab, bool showPanel = false) where T : MenuPanel
        {
            var classType = typeof(T);
            if (_menuPanels.ContainsKey(classType))
            {
                Debug.LogWarning($"Panel of {classType.Name} already exist.");
                return;
            }

            var panel = Instantiate(panelPrefab, transform);
            panel.gameObject.name = classType.Name;
            panel.Initialize(this);
            if (!showPanel) panel.HidePanel();
            _menuPanels.Add(classType, panel);
        }

        public T ShowPanel<T>(bool closeOtherPanels = true) where T : MenuPanel
        {
            var classType = typeof(T);
            if (!_menuPanels.ContainsKey(classType))
            {
                Debug.LogWarning("Failed to show non-existent panel.");
                return null;
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
            return panel as T;
        }

        public T HidePanel<T>() where T : MenuPanel
        {
            var classType = typeof(T);
            if (!_menuPanels.ContainsKey(classType))
            {
                Debug.LogWarning("Failed to hide non-existent panel.");
                return null;
            }
            
            var panel = _menuPanels[classType];
            panel.HidePanel();
            _shownPanels.Remove(panel);
            return panel as T;
        }

        public T GetPanel<T>() where T : MenuPanel
        {
            var classType = typeof(T);
            if (!_menuPanels.ContainsKey(classType))
            {
                Debug.LogWarning("Failed to hide non-existent panel.");
                return null;
            }
            
            return _menuPanels[classType] as T;
        }
    }
}
