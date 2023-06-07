using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public interface IReadOnlyEngineTimeCurrentTime
    {
        float Time { get; }
    }

    public interface IReadOnlyEngineTime : IReadOnlyEngineTimeCurrentTime
    {
        float DeltaTime { get; }
        float TimeScale { get; }
        float SecondsTo(float val);
        float SecondsFrom(float val);
        bool IsBefore(float val);
        bool IsBeforeOrSame(float val);
        bool IsAfter(float val);
        bool IsAfterOrSame(float val);
        void AddListener(float time, System.Action listener);
        void RemoveListener(float time, System.Action listener);
    }

    public interface IEngineTimeScale : IReadOnlyEngineTime
    {
        void SetTimeScale(float val);
    }

    public interface IEngineTime : IEngineTimeScale
    {
        void AddDeltaTime(float val);
        void SetTime(float val);
    }

    public class EngineTime : IEngineTime
    {
        private float time;
        public float Time => time;

        private float deltaTime = 0f;
        public float DeltaTime => deltaTime;

        private float timeScale = 1f;
        public float TimeScale => timeScale;

        private EngineTimeListenersData listenersData;

        public EngineTime(
            float time = 0,
            float deltaTime = 0,
            EngineTimeListenersData listenersData = null
        )
        {
            this.time = time;
            this.deltaTime = deltaTime;

            this.listenersData = listenersData ?? new EngineTimeListenersData();
            this.listenersData.SetEngineTime(this);
        }

        public void SetTime(float val)
        {
            if (val <= time) throw new System.Exception($"Value must be greater than current time (time = {time}, {val} given)");

            deltaTime = val - time;
            time = val;

            listenersData.InvokeListeners();
        }

        public void AddDeltaTime(float val)
        {
            deltaTime = val;
            time += val;

            listenersData.InvokeListeners();
        }

        public void SetTimeScale(float val)
        {
            timeScale = val;
        }

        public float SecondsTo(float val) { return val - time; }
        public float SecondsFrom(float val) { return time - val; }
        public bool IsBefore(float val) { return time < val; }
        public bool IsBeforeOrSame(float val) { return time <= val; }
        public bool IsAfter(float val) { return time > val; }
        public bool IsAfterOrSame(float val) { return time >= val; }

        public void AddListener(float time, System.Action listener) => listenersData.AddListener(time, listener);
        public void RemoveListener(float time, System.Action listener) => listenersData.RemoveListener(time, listener);
    }
}
