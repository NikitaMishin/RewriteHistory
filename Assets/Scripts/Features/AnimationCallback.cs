using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour {

    [SerializeField] private HiddenDoor hiddenDoor;

    public void OnEndOpen()
    {
        hiddenDoor.OnEndOpen();
    }

    public void OnEndClose()
    {
        hiddenDoor.OnEndClose();
    }

}
