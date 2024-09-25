using UnityEngine;
using Mirror;

public class Deck : NetworkBehaviour
{
    [Header("Player")]
    public Player player;
    [HideInInspector] public int deckSize = 30;
    [HideInInspector] public int handSize = 7;

    [Header("Decks")]
    public SyncListCard deckList = new SyncListCard(); // Danh sach bo bai chua tat ca la bai, rut bai o List nay.
    public SyncListCard graveyard = new SyncListCard(); // List bai bi tieu diet.
    public SyncListCard hand = new SyncListCard(); // Bai tren tay.

    [Header("Battlefield")]
    public SyncListCard playerField = new SyncListCard(); // Field.

    [Header("Starting Deck")]
    public CardAndAmount[] startingDeck;

    [HideInInspector] public bool spawnInitialCards = true;

    public void OnDeckListChange(SyncListCard.Operation op, int index, CardInfo oldCard, CardInfo newCard)
    {
        UpdateDeck(index, 1, newCard);
    }

    public void OnHandChange(SyncListCard.Operation op, int index, CardInfo oldCard, CardInfo newCard)
    {
        UpdateDeck(index, 2, newCard);
    }

    public void OnGraveyardChange(SyncListCard.Operation op, int index, CardInfo oldCard, CardInfo newCard)
    {
        UpdateDeck(index, 3, newCard);
    }

    public void UpdateDeck(int index, int type, CardInfo newCard)
    {
        if (type == 1) deckList[index] = newCard;
        if (type == 2) hand[index] = newCard;
        if (type == 3) graveyard[index] = newCard;

    }

    public bool CanPlayCard(int manaCost)
    {
        return player.mana >= manaCost && player.health > 0;
    }

    public void DrawCard(int amount)
    {
        PlayerHand playerHand = Player.gameManager.playerHand;
        for (int i = 0; i < amount; ++i)
        {
            int index = i;
            playerHand.AddCard(index);
        }
        spawnInitialCards = false;
    }

    [Command]
    public void CmdPlayCard(CardInfo card, int index)
    {
        CreatureCard creature = (CreatureCard)card.data;
        GameObject boardCard = Instantiate(creature.cardPrefab.gameObject);
        FieldCard newCard = boardCard.GetComponent<FieldCard>();
        newCard.card = new CardInfo(card.data); // Luu thong tin bai.
        newCard.cardName.text = card.name;
        newCard.health = creature.health;
        newCard.strength = creature.strength;
        newCard.image.sprite = card.image;
        newCard.image.color = Color.white;

        // Khong cho quai tan cong khi vua moi summon
        if (creature.hasCharge) newCard.waitTurn = 0;

        // Update thong tin quai khi di chuot qua
        newCard.cardHover.UpdateFieldCardInfo(card);

        NetworkServer.Spawn(boardCard);

        hand.RemoveAt(index);

        if (isServer) RpcPlayCard(boardCard, index);
    }

    [Command]
    public void CmdStartNewTurn()
    {
        if (player.mana < player.maxMana)
        {
            player.currentMax++;
            player.mana = player.currentMax;
            Debug.LogError("Here");
        }
    }

    [ClientRpc]
    public void RpcPlayCard(GameObject boardCard, int index)
    {
        if (Player.gameManager.isSpawning)
        {
            // Set up khong cho tu tan cong quan cua minh
            boardCard.GetComponent<FieldCard>().casterType = Target.FRIENDLIES;
            boardCard.transform.SetParent(Player.gameManager.playerField.content, false);
            Player.gameManager.playerHand.RemoveCard(index); // Update bai tren tay
            Player.gameManager.isSpawning = false;
        }
        else if (player.hasEnemy)
        {
            boardCard.GetComponent<FieldCard>().casterType = Target.ENEMIES;
            boardCard.transform.SetParent(Player.gameManager.enemyField.content, false);
            Player.gameManager.enemyHand.RemoveCard(index);
        }
    }
}
