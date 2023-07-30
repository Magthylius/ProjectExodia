using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExodia
{
    public class MainMenuPanel : MenuPanel
    {
        [SerializeField] private Animator panelAnimator;
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitButton;

        private bool _isLoading = false;
        
        private void Awake()
        {
            startButton.onClick.AddListener(OnStartButton);
            quitButton.onClick.AddListener(OnQuitButton);
            GameManager.OnGameRestart += () => _isLoading = false;
        }

        private void OnStartButton()
        {
            if (_isLoading) return;
            _isLoading = true;
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

        private void OnQuitButton()
        {
            /*if (_isLoading) return;
            _isLoading = true;
            BeginTransition();
            panelAnimator.SetInteger("ButtonMode", 2);
            
            IEnumerator Transition()
            {
                yield return new WaitForSeconds(1.5f);
                Application.Quit();
            }*/
        }
    }
}