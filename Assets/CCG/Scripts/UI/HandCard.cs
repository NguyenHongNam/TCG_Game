using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.XR;

public class HandCard : MonoBehaviour
{
    [Header("Sprite")]
    public Image image;

    [Header("Front & Back")]
    public Image cardfront;
    public Image cardback;

    [Header("Properties")]
    public Text cardName;
    public Text cost;
    public Text strength;
    public Text health;
    public Text description;
    public Text creatureType;

    [Header("Card Drag & Hover")]
    public HandCardDragHover cardDragHover;

    [Header("Outline")]
    public Image cardOutline;
    public Color readyColor;
    [HideInInspector] public int handIndex;
    [HideInInspector] public PlayerType playerType;

    public void AddCard(CardInfo newCard, int index, PlayerType playerT)
    {
        handIndex = index;
        playerType = playerT;

        cardDragHover.canHover = true;
        cardOutline.gameObject.SetActive(true);

        cardfront.color = Color.white;
        cardback.color = Color.clear;

        image.sprite = newCard.image;

        description.text = newCard.description; 
        cost.text = newCard.cost;
        cardName.text = newCard.name;

        if (newCard.data is CreatureCard)
        {
            health.text = ((CreatureCard)newCard.data).health.ToString();
            strength.text = ((CreatureCard)newCard.data).strength.ToString();
        }
    }

    public void AddCardBack()
    {
        cardfront.color = Color.clear;
        cardback.color = Color.white;
    }
    public void RemoveCard()
    {
        Destroy(gameObject);
    }

    public void UpdateFieldCardInfo(CardInfo card)
    {
        cardfront.color = Color.white;
        cardback.color = Color.clear;

        image.sprite = card.image;

        description.text = card.description;
        cost.text = card.cost;
        cardName.text = card.name;

        health.text = ((CreatureCard)card.data).health.ToString();
        strength.text = ((CreatureCard)card.data).strength.ToString();
    }

    private void Update()
    {
        if (playerType == PlayerType.PLAYER && cardDragHover != null)
        {
            Player player = Player.localPlayer;
            int manaCost = cost.text.ToInt();
            if (Player.gameManager.isOurTurn)
            {
                cardDragHover.canDrag = player.deck.CanPlayCard(manaCost);
                cardOutline.color = cardDragHover.canDrag ? readyColor : Color.clear;
            }
        }
    }
}