using System;
using UnityEngine;

#if UNITY_EDITOR
using Valve.VR.InteractionSystem;
#endif

namespace VRDemo
{
    public class InteractableItem : MonoBehaviour
    {
        [Header("Mesh")]
        [SerializeField]
        private MeshRenderer _objectMesh;

        [Header("Highlight")]
        [SerializeField]
        private Material _normalMaterial;

        [SerializeField]
        private Material _highlightedMaterial;

        [Header("Audio")]
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip _startGrabSound;

        [SerializeField]
        private AudioClip _stoptGrabSound;

#if UNITY_EDITOR
        private Interactable _interactableSteamVR;
#endif

        private bool _isHovering;
        private bool _isGrabbed;

        public event Action<InteractableItem, bool> GrabStateChangedListener;

        protected virtual void Awake()
        {
#if UNITY_EDITOR
            _interactableSteamVR = GetComponent<Interactable>();
#endif

            _audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            if (_interactableSteamVR != null)
            {
                if (_isHovering != _interactableSteamVR.isHovering)
                {
                    _isHovering = _interactableSteamVR.isHovering;
                    OnHoverStateChanged(_isHovering);
                }

                if (_isGrabbed != (_interactableSteamVR.attachedToHand != null))
                {
                    _isGrabbed = _interactableSteamVR.attachedToHand != null;
                    OnGrabStateChanged(_isGrabbed);
                }
            }
#endif
        }

        protected virtual void OnDisable()
        {
            Reset();
        }

        private protected void OnHoverStateChanged(bool isHovering)
        {
            _objectMesh.material = isHovering ? _highlightedMaterial : _normalMaterial;
        }

        private protected void OnGrabStateChanged(bool isGrabbed)
        {
            if (_audioSource != null)
            {
                _audioSource.clip = isGrabbed ? _startGrabSound : _stoptGrabSound;
                _audioSource.Play();
            }

            GrabStateChangedListener?.Invoke(this, isGrabbed);
        }

        public virtual void Reset()
        {
            GrabStateChangedListener = null;
        }
    }
}
