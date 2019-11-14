using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private int minPhysicalDmg;
    [SerializeField] private int maxPhysicalDmg;
    [SerializeField] private int minMagicDmg;
    [SerializeField] private int maxMagicDmg;
    [SerializeField] private int range;

    public string Name
    {
        get => name;
        set => name = value;
    }
}
