using CMCore.Interfaces;
using CMCore.Managers;
using UnityEngine;

namespace CMCore.UI
{
    public class UIBase : MonoBehaviour, IUI
    {
        public GameState state;
        public void Hide()
        {
            OnHide();
        }
        public void Show()
        {
            OnShow();
        }

        /// <summary>
        /// A Virtual Method that has been executed by Hide.
        /// </summary>
        protected virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// A Virtual Method that has been executed by Show.
        /// </summary>
        protected virtual void OnShow()
        {
            gameObject.SetActive(true);
        }
    }
}