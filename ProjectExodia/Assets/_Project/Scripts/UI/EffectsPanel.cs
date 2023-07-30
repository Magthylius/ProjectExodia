using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExodia
{
    public class EffectsPanel : MenuPanel
    {
        [SerializeField] private GameObject goBananaFireObject;
        [SerializeField] private GameObject passportStampingObject;
        [SerializeField] private GameObject stampedeObject;
        
        [SerializeField] private Image passportStampImage;
        [SerializeField] private Sprite alaskaStampSprite;
        [SerializeField] private Sprite antarcticaStampSprite;
        [SerializeField] private Sprite indiaStampSprite;
        [SerializeField] private Sprite japanStampSprite;
        [SerializeField] private Sprite malaysiaStampSprite;

        [SerializeField] private Animator stampedeAnimator;

        private void Awake()
        {
            goBananaFireObject.SetActive(false);
            LevelTransitionManager.OnCountryChange += OnCountryChange;
        }

        private void OnCountryChange(CountryPack countryPack)
        {
            SetPassportStamping(true);
        }

        public void SetGoBananaFire(bool active) => goBananaFireObject.SetActive(active);

        public void SetPassportStamping(bool active)
        {
            if (!passportStampingObject) return;
            
            passportStampingObject.SetActive(active);
            if (active)
            {
                switch (LevelTransitionManager.CurrentCountry)
                {
                    case ECountry.SOUTHPOLE: passportStampImage.sprite = antarcticaStampSprite; break;
                    case ECountry.INDIA: passportStampImage.sprite = indiaStampSprite; break;
                    case ECountry.JAPAN: passportStampImage.sprite = japanStampSprite; break;
                    case ECountry.MALAYSIA: passportStampImage.sprite = malaysiaStampSprite; break;
                    
                    default: passportStampImage.sprite = alaskaStampSprite; break;
                }

                StartCoroutine(AutoHide());
                
                IEnumerator AutoHide()
                {
                    yield return new WaitForSeconds(2f);
                    SetPassportStamping(false);
                }
            }
        }

        public void LevelUpStampede() => stampedeAnimator.SetTrigger("LevelUp");
        public void LevelDownStampede() => stampedeAnimator.SetTrigger("LevelDown");
    }
}
