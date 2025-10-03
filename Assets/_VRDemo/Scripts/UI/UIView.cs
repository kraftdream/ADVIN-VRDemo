using UnityEngine;

namespace VRDemo
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIView : MonoBehaviour
    {
        [Header("Base Options")]
        [SerializeField]
        private bool _visibleByDefault;

        [SerializeField]
        private bool _interactibleByDefault;

        private CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();

                return _canvasGroup;
            }
            set
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public float Visibility
        {
            get
            {
                return CanvasGroup.alpha;
            }
            set
            {
                CanvasGroup.alpha = value;
            }
        }

        public bool IsVisible => Visibility != 0;

        protected virtual void Awake()
        {
            if (!_visibleByDefault)
                HideView();
            else
                ShowView();

            CanvasGroup.interactable = _interactibleByDefault;
        }

        public virtual void ShowView()
        {
            gameObject.SetActive(true);
            CanvasGroup.alpha = 1.0f;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }

        public virtual void HideView()
        {
            CanvasGroup.alpha = 0.0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
    }
}
