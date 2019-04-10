using UnityEngine;


public abstract class BaseUserOfStateMachine : MonoBehaviour {
	//вынес это сюда на слючай если понадобится создать объект зависимый от двух состояний MultistateUserOfStateMachine : BaseUserOfStateMachine
	
	protected virtual void OnDisable() {
			//сработает при переходе на другую сцену
		AppStateManager.Instance.StateAppChanged -= SetActive;
	}

	protected abstract void SetActive(AppStateManager.StateApp previousState, AppStateManager.StateApp newState);

	protected virtual void OnGoingIntoState(){ }
	
	protected virtual void OnPassingFromState(){ }
}


