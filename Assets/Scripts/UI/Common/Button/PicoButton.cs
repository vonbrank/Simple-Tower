using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Common.Button
{
    public class PicoButton : UnityEngine.UI.Button
    {
        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(HandleClick);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            if (interactable)
            {
                HandlePointerEnter();
            }
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            if (interactable)
            {
                HandlePointerExit();
            }
        }

        void HandleClick()
        {
            DoStateTransition(SelectionState.Highlighted, false);
        }

        void HandlePointerEnter()
        {
            DoStateTransition(SelectionState.Highlighted, true);
        }

        void HandlePointerExit()
        {
            DoStateTransition(SelectionState.Normal, true);
        }
    }
}