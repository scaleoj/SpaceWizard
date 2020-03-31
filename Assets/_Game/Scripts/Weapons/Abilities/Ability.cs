using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public virtual void Attack(GameObject target, int distance) {}
    
    //ToolTip and purely visual Stuff
    [TextArea][SerializeField] private string abilityName;
    [TextArea][SerializeField] private string abilityDescription;
    
    //Parameter
    [SerializeField] private int aoeRange;
    [SerializeField] private int APCost;

    private Weapon parentWeapon;
    
    
    //GET,SET
    public int ApCost
    {
        get => APCost;
        set => APCost = value;
    }

    public int AoeRange
    {
        get => aoeRange;
        set => aoeRange = value;
    }

    public Weapon ParentWeapon
    {
        get => parentWeapon;
        set => parentWeapon = value;
    }

    public string AbilityName
    {
        get => abilityName;
        set => abilityName = value;
    }

    public string AbilityDescription
    {
        get => abilityDescription;
        set => abilityDescription = value;
    }
}
