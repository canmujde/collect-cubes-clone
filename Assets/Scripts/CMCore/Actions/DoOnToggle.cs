using UnityEngine;
using UnityEngine.Events;

namespace CMCore.Actions
{
    public class DoOnToggle : MonoBehaviour
    {
        public UnityEvent DoOnEnable;
        public UnityEvent DoOnDisable;
    

        private void OnEnable()
        {
            DoOnEnable?.Invoke();
        }

        private void OnDisable()
        {
            DoOnDisable?.Invoke();
        }
    
    }
}