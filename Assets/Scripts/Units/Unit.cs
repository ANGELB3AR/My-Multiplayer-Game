using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    [SerializeField] Targeter targeter = null;

    public override void OnStartClient()
    {
        gameObject.SetActive(true);
    }
}
