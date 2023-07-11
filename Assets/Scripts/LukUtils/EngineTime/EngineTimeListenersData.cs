using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EngineTime
{
    public class ListenersData
    {
        private IReadOnlyEngineTimeCurrentTime engineTime;

        private readonly SortedSet<float> listenerTimes = new SortedSet<float>(); public SortedSet<float> ListenerTimes => listenerTimes;
        private readonly ListDictionary<float, System.Action> listeners = new ListDictionary<float, System.Action>(); public IReadOnlyListDictionary<float, System.Action> Listeners => listeners;
        private readonly ListDictionary<System.Action, float> timesPerListener = new ListDictionary<System.Action, float>();

        // public ListenersData(
        //     IReadOnlyEngineTimeCurrentTime engineTime
        // )
        // {
        //     this.engineTime = engineTime;
        // }

        public void SetEngineTime(IReadOnlyEngineTimeCurrentTime engineTime)
        {
            this.engineTime = engineTime;
        }

        public void AddListener(float time, System.Action listener)
        {
            listenerTimes.Add(time);
            listeners.AddListItem(time, listener);
            timesPerListener.AddListItem(listener, time);
        }

        public void RemoveListener(float time, System.Action listener)
        {
            listeners.RemoveListItem(time, listener);
            timesPerListener.RemoveListItem(listener, time);

            if (listeners.IsEmpty(time))
            {
                listenerTimes.Remove(time);
            }
        }

        // ! @todo UT
        // @todo what if listeners and timesPerListener contain same listeners for the same time? memory leak as only the first occurrence is deleted?
        public void RemoveAllListeners(System.Action listener)
        {
            timesPerListener.TryGetValue(listener, out var times);

            if (times != null && times.Count > 0)
            {
                for (int i = 0; i < times.Count; i++)
                {
                    var time = times[i];

                    RemoveListener(time, listener);
                }
            }
        }

        public void InvokeListeners()
        {
            if (engineTime.Time < listenerTimes.Min) return;

            // var lastIndex = -1;
            // var i = 0;
            var toRemove = new List<float>();
            // for (int i = 0; i < listenerTimes.Count; i++)

            // var listenerTimesToInvoke = new SortedSet<float>(listenerTimes);
            var listenersToInvoke = new List<System.Action>();

            foreach (var listenerTime in listenerTimes)
            {
                // var listenerTime = listenerTimes.[i];

                if (engineTime.Time >= listenerTime)
                {
                    // lastIndex = i;

                    var listenersSlice = listeners[listenerTime];
                    listenersToInvoke.AddRange(listenersSlice);

                    listeners.Remove(listenerTime);
                    toRemove.Add(listenerTime);
                }
                else
                {
                    break;
                }

                // i += 1;
            }

            if (toRemove.Count > 0)
            {
                // listenerTimes.RemoveRange(0, lastIndex + 1);
                for (int j = 0; j < toRemove.Count; j++)
                {
                    listenerTimes.Remove(toRemove[j]);
                }
            }

            for (int i = 0; i < listenersToInvoke.Count; i++)
            {
                listenersToInvoke[i]();
            }
        }
    }
}
