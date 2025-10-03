using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;

namespace VRDemo
{
    public class CustomSteamVRLaserPointer : SteamVR_LaserPointer
    {
        [SerializeField]
        private LayerMask _interactLayers;

        [SerializeField]
        private bool _interaclOnlyPointer;

        private EventSystem _eventSystem;

        private void Awake()
        {
            _eventSystem = EventSystem.current;
            StartCoroutine(WaitUntilPointerExists());
        }

        private IEnumerator WaitUntilPointerExists()
        {
            yield return new WaitUntil(() => pointer != null);
            pointer.SetActive(!_interaclOnlyPointer);
        }

        public override void OnPointerIn(PointerEventArgs e)
        {
            if (((1 << e.target.gameObject.layer) & _interactLayers.value) != 0)
            {
                base.OnPointerIn(e);

                if (pointer != null && _interaclOnlyPointer)
                    pointer.gameObject.SetActive(true);

                IPointerEnterHandler pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();

                if (pointerEnterHandler == null)
                    return;

                pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));

                if (_eventSystem != null)
                    _eventSystem.SetSelectedGameObject(e.target.gameObject);
            }
        }

        public override void OnPointerClick(PointerEventArgs e)
        {
            if (((1 << e.target.gameObject.layer) & _interactLayers.value) != 0)
            {
                base.OnPointerClick(e);

                IPointerClickHandler clickHandler = e.target.GetComponent<IPointerClickHandler>();

                if (clickHandler == null)
                    return;

                clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }

        public override void OnPointerOut(PointerEventArgs e)
        {
            if (((1 << e.target.gameObject.layer) & _interactLayers.value) != 0)
            {
                base.OnPointerOut(e);

                if (pointer != null && _interaclOnlyPointer)
                    pointer.gameObject.SetActive(false);

                IPointerExitHandler pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();

                if (pointerExitHandler == null)
                    return;

                pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));

                if (_eventSystem != null)
                    _eventSystem.SetSelectedGameObject(null);
            }
        }
    }
}