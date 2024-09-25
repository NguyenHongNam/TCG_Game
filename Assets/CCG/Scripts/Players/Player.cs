using System;
using UnityEngine;
using Mirror;
using CameraShake;


public enum PlayerType { PLAYER, ENEMY };

[RequireComponent(typeof(Deck))]
[Serializable]
public class Player : Entity
{
    public BounceShake.Params shakeParams;
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
        // Update username
        username = newUser;
        gameObject.name = newUser;
    }

    [Command]
    public void CmdLoadDeck()
    {
        //Load bai vao bo bai
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

    [Command]
    public void CmdChangeHealth(int amount)
    {
        health += amount;
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
        if(Input.GetKeyDown(KeyCode.P)&& isLocalPlayer)
        {
            GameOverPopup.Instance.Show();
        }
        if (Input.GetKeyDown(KeyCode.Minus) && isLocalPlayer)
        {
            CmdChangeHealth(-1); // Giảm 1 máu
            Vector3 sourcePosition = transform.position;
            CameraShaker.Shake(new BounceShake(shakeParams, sourcePosition));
        }

        // Kiểm tra nếu phím 'H' được nhấn
        if (Input.GetKeyDown(KeyCode.H) && isLocalPlayer)
        {
            Debug.Log("Current Health: " + health); // In ra máu hiện tại
        }
    }

    public void UpdateEnemyInfo()
    {
        Player[] onlinePlayers = FindObjectsOfType<Player>();

        foreach (Player players in onlinePlayers)
        {
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