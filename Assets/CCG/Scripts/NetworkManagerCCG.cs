using UnityEngine;
using Mirror;


[AddComponentMenu("Network Manager CCG")]
public class NetworkManagerCCG : NetworkManager
{

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Transform startPos = GetStartPosition();
        GameObject player = Instantiate(playerPrefab);

        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
