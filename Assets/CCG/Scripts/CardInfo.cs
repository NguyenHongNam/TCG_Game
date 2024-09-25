using System;
using UnityEngine;
using Mirror;
using System.Collections.Generic;

[Serializable]
public partial struct CardInfo
{
   
    public string cardID;// Tao Id rieng cho moi la bai khong thi bi loi
    public int amount; // Build Deck(phat trien sau)

    public CardInfo(ScriptableCard data, int amount = 1)
    {
        cardID = data.CardID;
        this.amount = amount;
    }

    public ScriptableCard data
    {
        get
        {
            return ScriptableCard.Cache[cardID];
        }
    }

    public Sprite image => data.image;
    public string name => data.name;
    public string cost => data.cost.ToString();
    public string description => data.description;

    public List<Target> acceptableTargets => ((CreatureCard)data).acceptableTargets;
}

public class SyncListCard : SyncList<CardInfo> { }