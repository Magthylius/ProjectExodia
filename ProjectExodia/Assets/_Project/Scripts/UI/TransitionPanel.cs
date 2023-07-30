using System.Collections;
using UnityEngine;

namespace ProjectExodia
{
    public class TransitionPanel : MenuPanel
    {
        [SerializeField] private Animator panelAnimator;

        public void BeginTransition()
        {
            panelAnimator.SetTrigger("Begin");
            StartCoroutine(AutoHide());

            IEnumerator AutoHide()
            {
                yield return new WaitForSeconds(4.5f);
                UIManager.GetPanel<EffectsPanel>().SetPassportStamping(true);
                UIManager.HidePanel<TransitionPanel>();
            }
        }
    }
}
