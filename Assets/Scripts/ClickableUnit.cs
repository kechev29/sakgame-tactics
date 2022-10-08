using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableUnit : MonoBehaviour
{
    [SerializeField] PlayerInput pInput;
    Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    private void OnMouseUp()
    {
        pInput.TryQueueAttack(unit.tileX, unit.tileY, GetComponent<SoldierClass>());
    }


}
