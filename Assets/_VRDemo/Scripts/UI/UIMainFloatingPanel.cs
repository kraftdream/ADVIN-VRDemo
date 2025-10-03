using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VRDemo
{
    public class UIMainFloatingPanel : UIView
    {
        [SerializeField]
        private TMP_Text _attemptsText;

        [SerializeField]
        private TMP_Text _scoreText;

        [SerializeField]
        private Button _spawnThrowItemButton;

        [SerializeField]
        private Button _clearSceneButton;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _clickSound;

        [SerializeField]
        private AudioClip _hoverSound;

        [SerializeField]
        private AudioClip _unhoverSound;

        private int _attempts = 0;
        private float _score = 0;
        private GameObject _lastSelected;
        private EventSystem _eventSystem;

        public event Action SpawnThrowItemListener;

        public event Action ClearSceneListener;

        protected override void Awake()
        {
            base.Awake();

            _eventSystem = EventSystem.current;

            _spawnThrowItemButton.onClick.AddListener(OnSpawnThrowItemClicked);
            _clearSceneButton.onClick.AddListener(OnClearSceneClicked);
        }
        
        private void Update()
        {
            if (_eventSystem != null)
            {
                if (_lastSelected != _eventSystem.currentSelectedGameObject)
                {
                    _lastSelected = _eventSystem.currentSelectedGameObject;
                    AudioClip playSound = _unhoverSound;

                    if (_lastSelected == _spawnThrowItemButton.gameObject || _lastSelected == _clearSceneButton.gameObject)
                        playSound = _hoverSound;

                    _audioSource.clip = playSound;
                    _audioSource.Play();
                }
            }
        }

        private void OnSpawnThrowItemClicked()
        {
            SpawnThrowItemListener?.Invoke();

            _audioSource.clip = _clickSound;
            _audioSource.Play();
        }

        private void OnClearSceneClicked()
        {
            ClearSceneListener?.Invoke();

            _audioSource.clip = _clickSound;
            _audioSource.Play();
        }

        public int Attempts
        {
            get { return _attempts; }
            set
            {
                _attemptsText.text = $"Attempts: {value}";
                _attempts = value;
            }
        }

        public float Score
        {
            get { return _score; }
            set
            {
                _scoreText.text = $"Score: {value}";
                _score = value;
            }
        }
    }
}
