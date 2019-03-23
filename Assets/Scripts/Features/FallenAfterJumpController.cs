using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenAfterJumpController : MonoBehaviour {

    [SerializeField] private List<FallenAfterJumpBlock> fallenAfterJumpBlocks;
    [SerializeField] private Material marker;
    [SerializeField] private Material defaultMaterial;

    private bool _wasBroken = false;

    // Update is called once per frame
    void Update () {

        if (_wasBroken)
            return;

        int countCurrentBroken = 0;

        for (int i = 0; i < fallenAfterJumpBlocks.Count; i++)
        {
            if (fallenAfterJumpBlocks[i].IsBroken())
            {
                countCurrentBroken++;
                fallenAfterJumpBlocks[i].SetMaterial(marker);
            }
            else
            {
                fallenAfterJumpBlocks[i].SetMaterial(defaultMaterial);
            }
        }

        if (countCurrentBroken == fallenAfterJumpBlocks.Count)
        {
            SetGravity(true);
            _wasBroken = true;
        } 
        else
        {
            SetGravity(false);
            _wasBroken = false;
        }
	}

    private void SetGravity(bool value)
    {
        for (int i = 0; i < fallenAfterJumpBlocks.Count; i++)
        {
            fallenAfterJumpBlocks[i].SetGravity(value);
        }
    }

    public void SetWasBroken(bool value)
    {
        _wasBroken = value;
    }

    public bool WasBroken()
    {
        return _wasBroken;
    }
}
