using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] Targetable target = null;

    public Targetable GetTarget()
    {
        return target;
    }

    public void SetTarget(Targetable newTarget)
    {
        target = newTarget;
    }
}
