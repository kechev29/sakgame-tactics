using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] UnitsStats unitStats;

    private void OnMouseDown()
    {
        UIManager.Instance.UpdateUi(unitStats);
        UIManager.Instance.HighLightUnit(this);
    }
}
