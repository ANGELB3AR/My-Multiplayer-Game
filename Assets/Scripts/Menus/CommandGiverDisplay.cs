using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGiverDisplay : MonoBehaviour
{
    [SerializeField] RadialLayoutGroup radialLayoutGroup = null;
    [SerializeField] float radius = 60f;

    void Start()
    {
        radialLayoutGroup.Radius = radius;
    }
}
