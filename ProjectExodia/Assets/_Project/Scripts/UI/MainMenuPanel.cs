using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectExodia
{
    public class MainMenuPanel : MenuPanel
    {
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private Button startButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button tutorialOverlay;
        [SerializeField] private GameObject tutorialObject;

        private bool _isLoading = false;
        
        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButton);
            tutorialButton.onClick.AddListener(OnTutorialButton);
            tutorialOverlay.onClick.AddListener(OnTutorialOverlay);
            GameManager.OnGameRestart += () => _isLoading = false;
        }

        private void OnStartButton()
        {
            if (_isLoading) return;
            _isLoading = true;
            tutorialObject.SetActive(false);
            StartCoroutine(Transition());
            panelAnimator.SetInteger("ButtonMode", 1);
            
            IEnumerator Transition()
            {
                yield return new WaitForSeconds(1.5f);
                UIManager.ShowPanel<TransitionPanel>().BeginTransition();

                yield return new WaitForSeconds(1.5f);
                UIManager.ShowPanel<MainHUDPanel>(false);
                UIManager.HidePanel<MainMenuPanel>();
                GameManager.GameState = GameState.Gameplay;
                panelAnimator.SetTrigger("ResetButton");
            }
        }

        private void OnTutorialButton()
        {
            tutorialObject.SetActive(true);
        }

        private void OnTutorialOverlay()
        {
            tutorialObject.SetActive(false);
        }
    }
}