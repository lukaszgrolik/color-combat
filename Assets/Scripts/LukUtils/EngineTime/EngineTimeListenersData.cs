using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class EngineTimeListenersData
    {
        private IReadOnlyEngineTimeCurrentTime engineTime;

        private readonly SortedSet<float> listenerTimes = new SortedSet<float>(); public SortedSet<float> ListenerTimes => listenerTimes;
        private readonly ListDictionary<float, System.Action> listeners = new ListDictionary<float, System.Action>(); public IReadOnlyListDictionary<float, System.Action> Listeners => listeners;

        // public EngineTimeListenersData(
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
        }

        // @todo test
        public void RemoveListener(float time, System.Action listener)
        {
            listeners.RemoveListItem(time, listener);

            if (listeners[time].Count == 0)
            {
                listenerTimes.Remove(time);
            }
        }

        public void InvokeListeners()
        {
            if (engineTime.Time < listenerTimes.Min) return;

            // var lastIndex = -1;
            // var i = 0;
            var toRemove = new List<float>();
            // for (int i = 0; i < listenerTimes.Count; i++)
            foreach (var listenerTime in listenerTimes)
            {
                // var listenerTime = listenerTimes.[i];

                if (engineTime.Time >= listenerTime)
                {
                    // lastIndex = i;

                    var listenersSlice = listeners[listenerTime];

                    for (int j = 0; j < listenersSlice.Count; j++)
                    {
                        listenersSlice[j]();
                    }

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
        }
    }
}
