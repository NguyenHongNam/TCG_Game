using System;
using System.Collections.Generic;

public enum AbilityType : byte { DAMAGE, HEAL, DRAW, DISCARD, BUFF, DEBUFF }

[Serializable]
public struct CardAbility
{
    public AbilityType abilityType;
    public List<Target> targets;
    public ScriptableAbility ability;
}