using System;
using System.Linq;
using UnityEngine;

public sealed class AppStateManager : Singleton<AppStateManager> {
#if UNITY_EDITOR
    public bool isDebug = true;
#endif

    public enum StateApp {
        Change
    }

    public delegate void StateChange(StateApp previousState, StateApp newState);

    public event StateChange StateAppChanged;

    public StateApp LastState { get; private set; } = StateApp.Change;

    StateApp _currentlyApplicationState = StateApp.Change;

    public StateApp CurrentlyApplicationState {
        get { return _currentlyApplicationState; }
        private set {
            if (_currentlyApplicationState == value) 
                return;
            
            LastState = _currentlyApplicationState;
            _currentlyApplicationState = value;
            OnStateAppChanged(LastState, _currentlyApplicationState);
        }
    }

    public void SetState(int state) {
        //это если хочется задать состояние через инспектор
        if (StateExist(state))
            CurrentlyApplicationState = (StateApp) state;
        else
            throw new Exception("Неизвестное состояние");
    }

    public void SetState(StateApp state) {
        if (CurrentlyApplicationState != state)
            CurrentlyApplicationState = state;
        else
            throw new Exception("error: newState = currState");
    }

    static bool StateExist(int state) {
        string nameState = ((StateApp) state).ToString();
        return Enum.GetNames(typeof(StateApp)).Contains(nameState);
    }

    void OnStateAppChanged(StateApp previousState, StateApp newSate) {
        StateAppChanged?.Invoke(previousState, newSate);
        
#if UNITY_EDITOR
        if (isDebug)
            Debug.Log($"previousState: {previousState}; newsSate: {newSate}");
#endif
    }
}