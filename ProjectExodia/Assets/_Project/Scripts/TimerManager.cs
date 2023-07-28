using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public class TimerManager : ManagerBase
    {
        private readonly Dictionary<Guid, Coroutine> _timerObjects = new();
        public int TimerCount => _timerObjects.Count;
        
        public Guid CreateTimer(Action tickedDelegate, float interval, bool isLooping = false)
        {
            var timerID = Guid.NewGuid();
            if (!isLooping)
            {
                tickedDelegate += () => StopTimer(timerID);
            }
            
            IEnumerator TimerRoutine()
            {
                do
                {
                    yield return new WaitForSeconds(interval);
                    tickedDelegate?.Invoke();
                    
                    // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                } while (isLooping);
            }

            _timerObjects.Add(timerID, StartCoroutine(TimerRoutine()));
            return timerID;
        }
        
        public void StopTimer(Guid timerID)
        {
            if (!_timerObjects.ContainsKey(timerID))
            {
                Debug.LogWarning($"TimerManager: Invalid timer ID of {timerID}, timer counts {_timerObjects.Count}.");
                return;
            }
            
            StopCoroutine(_timerObjects[timerID]);
            _timerObjects.Remove(timerID);
        }
    }
}
