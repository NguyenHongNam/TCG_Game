using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class FieldCard : Entity
{
    [SyncVar, HideInInspector] public CardInfo card; // Get card info

    [Header("Card Properties")]
    public Image image;
    public Text cardName;
    public Text healthText;
    public Text strengthText;

    [Header("Shine")]
    public Image shine;
    public Color hoverColor;
    public Color readyColor;
    public Color targetColor;

    [Header("Card Hover")]
    public HandCard cardHover;

    public override void Update()
    {
        base.Update();
     
        if (image.sprite == null && (card.name != null || cardName.text == ""))
        {
            image.color = Color.white;
            image.sprite = card.image;
            cardName.text = card.name;

            cardHover.UpdateFieldCardInfo(card);
        }

        healthText.text = health.ToString();
        strengthText.text = strength.ToString();

        if (CanAttack()) shine.color = readyColor;
        else if (CantAttack()) shine.color = Color.clear;
    }

    [Command(ignoreAuthority = true)]
    public void CmdUpdateWaitTurn()
    {
        Debug.LogError("Here");
        if (waitTurn > 0) waitTurn--;
    }
}