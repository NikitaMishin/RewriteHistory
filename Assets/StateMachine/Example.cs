using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : UserOfStateMachine {
    protected override AppStateManager.StateApp GetMainState() {
        return AppStateManager.StateApp.Change;
    }


    protected override void OnGoingIntoState() {
        base.OnGoingIntoState();
    }

    protected override void OnPassingFromState() {
        base.OnPassingFromState();
    }
}
