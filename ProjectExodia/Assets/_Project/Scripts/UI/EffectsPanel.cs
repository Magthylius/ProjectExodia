using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExodia
{
    public class EffectsPanel : MenuPanel
    {
        [SerializeField] private GameObject goBananaFireObject;
        [SerializeField] private GameObject passportStampingObject;
        
        [SerializeField] private Image passportStampImage;
        [SerializeField] private Sprite alaskaStampSprite;
        [SerializeField] private Sprite antarcticaStampSprite;
        [SerializeField] private Sprite indiaStampSprite;
        [SerializeField] private Sprite japanStampSprite;
        [SerializeField] private Sprite malaysiaStampSprite;
        

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
    }
}
