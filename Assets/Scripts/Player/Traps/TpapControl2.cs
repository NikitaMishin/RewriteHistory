using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpapControl2 : MonoBehaviour {

    [SerializeField]
    private GameObject _respawn;
    //this script only for this lvl because we have only 1 respawn point
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Trap1"))
        {
            GoToRespawn();
        }
    }

    private void GoToRespawn()
    {
        gameObject.transform.position = new Vector3(
            _respawn.transform.position.x,
            _respawn.transform.position.y + 2,
            _respawn.transform.position.z
        );
    }
}


