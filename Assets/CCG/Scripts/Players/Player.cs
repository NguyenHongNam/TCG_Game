using System;
using UnityEngine;
using Mirror;

public enum PlayerType { PLAYER, ENEMY };

[RequireComponent(typeof(Deck))]
[Serializable]
public class Player : Entity
{
    [Header("Player Info")]
    [SyncVar(hook = nameof(UpdatePlayerName))] public string username;

    [Header("Portrait")]
    public Sprite portrait;

    [Header("Deck")]
    public Deck deck;
    public Sprite cardback;
    [SyncVar, HideInInspector] public int tauntCount = 0;

    [Header("Stats")]
    [SyncVar] public int maxMana = 10;
    [SyncVar] public int currentMax = 0;
    [SyncVar] public int _mana = 0;
    public int mana
    {
        get { return Mathf.Min(_mana, maxMana); }
        set { _mana = Mathf.Clamp(value, 0, maxMana); }
    }

    [HideInInspector] public static Player localPlayer;
    [HideInInspector] public bool hasEnemy = false;
    [HideInInspector] public PlayerInfo enemyInfo;
    // We store all our enemy's info in a PlayerInfo struct so we can pass it through the network when needed.
    [HideInInspector] public static GameManager gameManager;
    [SyncVar, HideInInspector] public bool firstPlayer = false;

    public override void OnStartLocalPlayer()
    {
        localPlayer = this;

        CmdLoadPlayer(PlayerPrefs.GetString("Name"));
        CmdLoadDeck();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        deck.deckList.Callback += deck.OnDeckListChange;
        deck.graveyard.Callback += deck.OnGraveyardChange;
    }

    [Command]
    public void CmdLoadPlayer(string user)
    {
        username = user;
    }

   
    void UpdatePlayerName(string oldUser, string newUser)
    {
        username = newUser;
        gameObject.name = newUser;
    }

    [Command]
    public void CmdLoadDeck()
    {
        // Fill deck from startingDeck array
        for (int i = 0; i < deck.startingDeck.Length; ++i)
        {
            CardAndAmount card = deck.startingDeck[i];
            for (int v = 0; v < card.amount; ++v)
            {
                deck.deckList.Add(card.amount > 0 ? new CardInfo(card.card, 1) : new CardInfo());
                if (deck.hand.Count < 7) deck.hand.Add(new CardInfo(card.card, 1));
            }
        }
        if (deck.hand.Count == 7)
        {
            deck.hand.Shuffle();
        }
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        health = gameManager.maxHealth;
        maxMana = gameManager.maxMana;
        deck.deckSize = gameManager.deckSize;
        deck.handSize = gameManager.handSize;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
 
        if (!hasEnemy && username != "")
        {
            UpdateEnemyInfo();
        }

        if (Input.GetKeyDown(KeyCode.G) && isLocalPlayer)
        {
            gameManager.StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Minus) && isLocalPlayer)
        {
            CmdReduceHealth(1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameEndPopup.Instance.WinningPopup();
        }

        // Kiểm tra phím L để hiện popup thua
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameEndPopup.Instance.LoosingPopup();
        }
    }

    [Command]
    public void CmdReduceHealth(int amount)
    {
        // Trừ máu người chơi
        health -= amount;
    }
    public void UpdateEnemyInfo()
    {
        // Find all Players and add them to the list.
        Player[] onlinePlayers = FindObjectsOfType<Player>();

        // Loop through all online Players (should just be one other Player)
        foreach (Player players in onlinePlayers)
        {
            // Make sure the players are loaded properly (we load the usernames first)
            if (players.username != "")
            {
                if (players != this)
                {
                    PlayerInfo currentPlayer = new PlayerInfo(players.gameObject);
                    enemyInfo = currentPlayer;
                    hasEnemy = true;
                    enemyInfo.data.casterType = Target.OPPONENT;
                }
            }
        }
    }

    public bool IsOurTurn() => gameManager.isOurTurn;
}