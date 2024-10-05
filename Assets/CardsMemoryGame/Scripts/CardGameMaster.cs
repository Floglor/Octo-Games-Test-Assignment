using System.Collections.Generic;
using Naninovel;
using UnityEngine;
using UnityEngine.UI;

namespace CardsMemoryGame.Scripts
{
    public class CardGameMaster : MonoBehaviour
    {
        [SerializeField] private List<RotatingCard> _rotatingCards;
        [SerializeField] private GameObject _rotatingCardPrefab;
        [SerializeField] private List<Sprite> _cardsSprites;
        [SerializeField] private Sprite _cardsBackSprite;

        [SerializeField] private int numberOfCards = 6;
        private RotatingCard _firstFlippedCard = null;
        private RotatingCard _secondFlippedCard = null;
        private int _mistakes = 0;
        private float _elapsedTime = 0f;
        private bool _gameStarted = false;

        private bool _flipLocked = false;

        private void Update()
        {
            if (_gameStarted)
            {
                _elapsedTime += Time.deltaTime;
            }
        }

        private async void GoBackToNovelAsync()
        {
            IntegerParameter mistakes = new IntegerParameter
            {
                Value = _mistakes
            };

            IntegerParameter time = new IntegerParameter
            {
                Value = (int) _elapsedTime
            };
            
            SwitchToNovel switchCommand = new SwitchToNovel
            {
                PlaybackSpot = default,
                ScriptName = "GameResult",
                Mistakes = mistakes,
                Time = time,
            };
            await switchCommand.ExecuteAsync();
        }

        public void StartGame(int cardsNumber)
        {
            numberOfCards = cardsNumber;
            InitializePlayingCards();
        }

     //  private async void Start()
     //  {
     //      SwitchToMinigame switchCommand = new SwitchToMinigame();
     //      ICustomVariableManager variableManager = Engine.GetService<ICustomVariableManager>(); 
     //      variableManager.TryGetVariableValue<int>("MinigameCards", out int intValue);
     //      numberOfCards = intValue;
     //      InitializePlayingCards();
     //      await switchCommand.ExecuteAsync();
     //  }

        private void InitializePlayingCards()
        {
            if (numberOfCards < 0 || numberOfCards % 2 != 0)
            {
                Debug.LogError("Number of cards in Card Memory Game is not even or less than zero.");
            }

            // ReSharper disable once ForCanBeConvertedToForeach
            for (int i = 0; i < _rotatingCards.Count; i++)
            {
                RotatingCard card = _rotatingCards[i];
                Destroy(card.gameObject);
            }

            _rotatingCards.Clear();

            List<Sprite> cardSpritesForGame = GetShuffledCardSprites();

            for (int i = 0; i < numberOfCards; i++)
            {
                GameObject cardObject = Instantiate(_rotatingCardPrefab, transform);
                RotatingCard card = cardObject.GetComponent<RotatingCard>();

                card.SetCardSprites(_cardsBackSprite, cardSpritesForGame[i]);
                card.OnFlipOver += OnCardFlipped;
                card.GetComponent<Button>().onClick.AddListener(delegate { CardClicked(card); });

                _rotatingCards.Add(card);
            }

            _gameStarted = true;
            _elapsedTime = 0f;
            _mistakes = 0;
        }

        private void CardClicked(RotatingCard card)
        {
            if (_flipLocked) return;
            if (!card.IsCardBackToggled || card.IsSolved) return;

            card.FlipCard();
            _flipLocked = true;
        }

        private List<Sprite> GetShuffledCardSprites()
        {
            List<Sprite> cardSpritesForGame = new List<Sprite>();

            for (int i = 0; i < numberOfCards / 2; i++)
            {
                cardSpritesForGame.Add(_cardsSprites[i]);
                cardSpritesForGame.Add(_cardsSprites[i]);
            }

            for (int i = 0; i < cardSpritesForGame.Count; i++)
            {
                Sprite temp = cardSpritesForGame[i];
                int randomIndex = UnityEngine.Random.Range(0, cardSpritesForGame.Count);
                cardSpritesForGame[i] = cardSpritesForGame[randomIndex];
                cardSpritesForGame[randomIndex] = temp;
            }

            return cardSpritesForGame;
        }

        private void OnCardFlipped(RotatingCard card)
        {
            if (_firstFlippedCard == null)
            {
                _firstFlippedCard = card;
            }
            else if (_secondFlippedCard == null)
            {
                _secondFlippedCard = card;

                LockAllCards();

                CheckCardsMatch();
            }

            UnlockAllCards();
        }

        private void CheckCardsMatch()
        {
            if (_firstFlippedCard.CompareSprite(_secondFlippedCard.GetCardSprite()))
            {
                _firstFlippedCard.SolveCard();
                _secondFlippedCard.SolveCard();

                CheckForWin();
                UnlockAllCards();
            }
            else
            {
                _mistakes++;
                UnlockAllCards();
                _firstFlippedCard.FlipCard();
                _secondFlippedCard.FlipCard();
            }


            _firstFlippedCard = null;
            _secondFlippedCard = null;
        }

        private void CheckForWin()
        {
            bool gameWon = true;
            foreach (RotatingCard rotatingCard in _rotatingCards)
            {
                if (!rotatingCard.IsSolved) gameWon = false;
            }

            if (gameWon)
            {
                _gameStarted = false;
                GoBackToNovelAsync();
            }
            else
            {
                //NotYet
            }
        }

        private void LockAllCards()
        {
            _flipLocked = true;
        }

        private void UnlockAllCards()
        {
            _flipLocked = false;
        }

      //  private void OnEnable()
      //  {
      //      InitializePlayingCards();
      //  }

        public int GetMistakes()
        {
            return _mistakes;
        }

        public float GetElapsedTime()
        {
            return _elapsedTime;
        }
    }
}