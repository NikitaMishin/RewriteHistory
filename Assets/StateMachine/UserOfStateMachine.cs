using UnityEngine;


public abstract class UserOfStateMachine : BaseUserOfStateMachine {
    protected virtual void Start() {
        AppStateManager.Instance.StateAppChanged += SetActive;
        
        
#if UNITY_EDITOR
        if (AppStateManager.Instance.isDebug)
            Debug.Log(gameObject.name + " подписался на StateAppChanged");
#endif
    }

    protected sealed override void SetActive(AppStateManager.StateApp previousState, AppStateManager.StateApp newState) {
        AppStateManager.StateApp mainState = GetMainState();
        
        if (newState == mainState) 
            OnGoingIntoState();
        
        else if (previousState == mainState)
            OnPassingFromState();
    }

    /// <summary>
    ///   <para>При каком состоянии нужно вызвать OnPassingFromState()</para>
    ///   <para>Пример: return ApplicationStateManager.StateApp.StateName</para>
    /// </summary>
    protected abstract AppStateManager.StateApp GetMainState();
}
