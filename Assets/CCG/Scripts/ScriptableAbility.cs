
using UnityEngine;
using System.Collections.Generic;


public partial class ScriptableAbility : ScriptableObject
{
    [Header("Targets")]
    public List<Target> targets = new List<Target>();

    [Header("Damage or Heal")]
    public int damage = 0;
    public int heal = 0;

    [Header("Buffs and Debuffs")]
    public int strength = 0;
    public int health = 0;

    [Header("Draw or Discard")]
    public int draw = 0;
    public int discard = 0;

    [Header("Properties")]
    public bool untilEndOfTurn = false;

    public virtual void Cast(Entity target)
    {

    }

    private void OnValidate()
    {
        
        if (targets.Count == 0)
        {
            targets.Add(Target.OPPONENT);
        }
    }
}