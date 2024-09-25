
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Card/Spell Card", order = 111)]
public partial class SpellCard : ScriptableCard
{
    [Header("Propeties")]
    public bool targeted = false;
    public int healthChange = 0;
    public int strengthChange = 0;
    public int cardDraw = 0;
    public bool untilEndOfTurn = false;
}