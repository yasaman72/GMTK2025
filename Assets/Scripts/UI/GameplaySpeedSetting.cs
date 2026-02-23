using UnityEngine;
using UnityEngine.UI;

namespace Deviloop
{
    public class GameplaySpeedSetting : MonoBehaviour, IInitiatable
    {
        public static float GameplaySpeed { get; set; }
        [SerializeField] private TMPro.TMP_Text _gameplaySpeedText;
        [SerializeField] private float _speedChangeStep = 0.5f;
        [SerializeField] private int _maxSpeed = 3, _minSpeed = 1;
        [SerializeField] private Button _speedIncreaseBtn, _speedDecreaseBtn;

        private const string _gameplaySpeedKey = "GameplaySpeed";


        public void Initiate()
        {
            if (PlayerPrefs.HasKey(_gameplaySpeedKey))
            {
                GameplaySpeed = PlayerPrefs.GetFloat(_gameplaySpeedKey, 1f);
            }
            else
            {
                GameplaySpeed = 1;
                PlayerPrefs.SetFloat(_gameplaySpeedKey, GameplaySpeed);
            }
        }

        public void Deactivate()
        {
        }

        private void Start()
        {
            _speedIncreaseBtn.onClick.AddListener(IncreaseGameSpeed);
            _speedDecreaseBtn.onClick.AddListener(DecreaseGameSpeed);
        }

        private void OnEnable()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            _gameplaySpeedText.text = $"{GameplaySpeed:0.0}x";
            _speedIncreaseBtn.interactable = GameplaySpeed < _maxSpeed;
            _speedDecreaseBtn.interactable = GameplaySpeed > _minSpeed;
        }

        public void IncreaseGameSpeed()
        {
            if (GameplaySpeed < _maxSpeed)
            {
                GameplaySpeed += _speedChangeStep;
                PlayerPrefs.SetFloat(_gameplaySpeedKey, GameplaySpeed);
                UpdateUI();
            }
        }

        public void DecreaseGameSpeed()
        {
            if (GameplaySpeed > _minSpeed)
            {
                GameplaySpeed -= _speedChangeStep;
                PlayerPrefs.SetFloat(_gameplaySpeedKey, GameplaySpeed);
                UpdateUI();
            }
        }
    }
}
