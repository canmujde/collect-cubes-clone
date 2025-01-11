using System;
using CMCore.Util;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CMCore.Managers
{
    public class InputManager : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IEndDragHandler
    {
        public static event Action<PointerEventData> PointerDown;
        public static event Action<PointerEventData> PointerDrag;
        public static event Action<PointerEventData> PointerDragEnd;
        public static event Action<PointerEventData> PointerUp;
        
        public bool InputAllowed { get; private set; }

   
        private GameManager _gameManager;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
            InputAllowed = false;
            EventManager.OnGameStateChanged += OnGameStateChanged;

        }


        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    InputAllowed = true;
                    break;
                case GameState.InGame:
                    // this.DelayedAction(ToggleInput, 0.5f);
                    break;
                case GameState.Fail:
                    InputAllowed = false;
                    break;
                case GameState.Win:
                    InputAllowed = false;
                    break;
            }
        }
        public void OnPointerDown(PointerEventData pointerEventData)
        {
            if (!InputAllowed) return;
            PointerDown?.Invoke(pointerEventData);
        }

        public void OnDrag(PointerEventData pointerEventData)
        {
            if (!InputAllowed) return;
            PointerDrag?.Invoke(pointerEventData);
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!InputAllowed) return;
            PointerDragEnd?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData pointerEventData)
        {
            if (!InputAllowed) return;
            PointerUp?.Invoke(pointerEventData);
        }


        private void ToggleInput() =>InputAllowed = !InputAllowed;
        
    }
}