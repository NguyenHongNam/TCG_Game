
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct CardAndAmount
{
    public ScriptableCard card;
    public int amount;
}


public partial class ScriptableCard : ScriptableObject
{
    [SerializeField] string id = "";
    public string CardID { get { return id; } }

    [Header("Image")]
    public Sprite image; // Card image

    [Header("Properties")]
    public int cost;
    public string category;

    [Header("Initiative Abilities")]
    public List<CardAbility> intiatives = new List<CardAbility>();

    [HideInInspector] public bool hasInitiative = false;

    [Header("Description")]
    [SerializeField, TextArea(1, 30)] public string description;

    
    static Dictionary<string, ScriptableCard> _cache;
    public static Dictionary<string, ScriptableCard> Cache
    {
        get
        {
            if (_cache == null)
            {
                ScriptableCard[] cards = Resources.LoadAll<ScriptableCard>("");

                _cache = cards.ToDictionary(card => card.CardID, card => card);
            }
            return _cache;
        }
    }

    public virtual void Cast(Entity caster, Entity target)
    {

    }

    private void OnValidate()
    {
        if (CardID == "")
        {
#if UNITY_EDITOR
            string path = AssetDatabase.GetAssetPath(this);
            id = AssetDatabase.AssetPathToGUID(path);
#endif
        }

        if (intiatives.Count > 0) hasInitiative = true;
    }
}