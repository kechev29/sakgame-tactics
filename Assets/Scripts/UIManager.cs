using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text unitName;
    [SerializeField] private Text unitDescription;
    [SerializeField] private Text health;
    [SerializeField] private Text attack;
    [SerializeField] private Text defense;
    [SerializeField] private Text movement;
    [SerializeField] private Text action;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }

    public void UpdateUi(UnitsStats unitsStats)
    {
        unitName.text = unitsStats.UnitName;
        unitDescription.text = unitsStats.UnitDescription;
        health.text = unitsStats.HealthPoints.ToString();
        attack.text = unitsStats.AttackPoints.ToString();
        defense.text = unitsStats.DefensePoints.ToString();
        movement.text = unitsStats.MovementPoints.ToString();
        action.text = unitsStats.ActionPoints.ToString();
    }

    public void HighLightUnit(Unit unitToHighLight)
    {
       Unit[] units = FindObjectsOfType<Unit>();
        foreach (Unit unit in units)
        {
            unit.GetComponent<SpriteRenderer>().color = Color.white;
        }
        unitToHighLight.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
