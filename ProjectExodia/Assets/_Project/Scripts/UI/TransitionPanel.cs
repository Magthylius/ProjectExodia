using System.Collections;
using UnityEngine;

namespace ProjectExodia
{
    public class TransitionPanel : MenuPanel
    {
        public delegate void TransitionEvent();
        public event TransitionEvent OnFullTransition;
        
        [SerializeField] private Animator panelAnimator;

        public void BeginTransition()
        {
            GameManager.GameState = GameState.Transition;
            panelAnimator.SetTrigger("Begin");
            StartCoroutine(AutoHide());

            IEnumerator AutoHide()
            {
                //! yield return has to total up to 4.5f
                yield return new WaitForSeconds(1.5f);
                OnFullTransition?.Invoke();
                
                yield return new WaitForSeconds(3.0f);
                if (GameContext.Instance.TryGetManager(out GameManager gameManager))
                {
                    gameManager.StartGameplay();
                }
                
                UIManager.GetPanel<EffectsPanel>().SetPassportStamping(true);
                UIManager.HidePanel<TransitionPanel>();
            }
        }
    }
}
