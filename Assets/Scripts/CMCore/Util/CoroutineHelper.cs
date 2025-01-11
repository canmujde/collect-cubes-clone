using System;
using System.Collections;
using UnityEngine;

namespace CMCore.Util
{
    public static class CoroutineHelper
    {
        public static void DelayedAction(MonoBehaviour holdLive, Action action, float delay, bool realtime = false)
        {
            holdLive.StartCoroutine(DelayedActionRoutine(action, delay, realtime));
        }

        private static IEnumerator DelayedActionRoutine(Action action, float delay, bool realtime)
        {
            if (realtime)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }
            action?.Invoke();
        }
    }
}