using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public virtual void Attack(GameObject origin, GameObject[] target, Weapon weapon, int distance) {}
}
