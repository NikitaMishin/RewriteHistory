using System.Collections.Generic;
using UnityEngine;

public abstract class MultistateUserOfStateMachine : BaseUserOfStateMachine {

    [Header("В каких состояниях объект активен: ")] [SerializeField]
    protected List<AppStateManager.StateApp> TargetStates;

    protected virtual void Start() {
        if (TargetStates.Count > 0) 
            AppStateManager.Instance.StateAppChanged += SetActive;
        else 
            Debug.LogError("У объекта " + gameObject.name + " не перечислены состояния при которых он активен");
    }

    protected override void SetActive(AppStateManager.StateApp previousState, AppStateManager.StateApp newState) {
        if (TargetStates.Contains(newState)/* && !TargetStates.Contains(previousState)*/) 
            OnGoingIntoState(newState, previousState);
        else 
            OnPassingFromState(newState,previousState);
    }
 
    protected virtual void OnGoingIntoState(AppStateManager.StateApp previousState, AppStateManager.StateApp newState) {
    }

    protected virtual void OnPassingFromState(AppStateManager.StateApp previousState, AppStateManager.StateApp newState) {
    }
}
