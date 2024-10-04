using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CardsMemoryGame.Scripts
{
    public class RotatingCard : MonoBehaviour
    {
        private const float TURN_RATE_WAIT_TIME = 0.0025f;
        private static readonly Vector3 rotationVector = new Vector3(0, 1, 0);

        [SerializeField] private Sprite _cardSprite;
        [SerializeField] private Sprite _cardBackSprite;

        [SerializeField] private Image _cardImage;

        public Action<RotatingCard> OnFlipOver;

        [HideInInspector] public bool IsCardBackToggled = true;

        public bool IsSolved;

        private void ToggleCardSprite()
        {
            if (IsCardBackToggled)
            {
                _cardImage.sprite = _cardSprite;
                IsCardBackToggled = false;
            }
            else
            {
                _cardImage.sprite = _cardBackSprite;
                IsCardBackToggled = true;
            }
        }


        private IEnumerator Flip()
        {
            bool turned = false;
            for (int i = 0; i < 180; i++)
            {
                yield return new WaitForSeconds(TURN_RATE_WAIT_TIME);

                if (!turned)
                {
                    transform.Rotate(rotationVector);
                }
                else
                {
                    transform.Rotate(-rotationVector);
                }

                if (i == 89)
                {
                    ToggleCardSprite();
                    turned = true;
                }

                if (i == 179)
                {
                
                    if (!IsCardBackToggled)
                        OnFlipOver?.Invoke(this);
                }
            }
        }

        public void FlipCard()
        {
            if (IsSolved) return;

            StartCoroutine(Flip());
        }

        public bool CompareSprite(Sprite sprite)
        {
            return _cardImage.sprite == sprite;
        }

        public void SetCardSprites(Sprite cardBackSprite, Sprite cardSprite)
        {
            _cardSprite = cardSprite;
            _cardBackSprite = cardBackSprite;

            _cardImage.sprite = cardBackSprite;
        }
        public void SolveCard()
        {
            GetComponent<Button>().interactable = false;
            IsSolved = true;
        }

        public Sprite GetCardSprite()
        {
            return _cardImage.sprite;
        }
    }
}