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
        }

        public void SetGoBananaFire(bool active) => goBananaFireObject.SetActive(active);

        public void SetPassportStamping(bool active, int regionType = -1)
        {
            passportStampingObject.SetActive(active);
            if (active)
            {
                switch (regionType)
                {
                    case 0: passportStampImage.sprite = alaskaStampSprite; break;
                    case 1: passportStampImage.sprite = antarcticaStampSprite; break;
                    case 2: passportStampImage.sprite = indiaStampSprite; break;
                    case 3: passportStampImage.sprite = japanStampSprite; break;
                    case 4: passportStampImage.sprite = malaysiaStampSprite; break;
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
