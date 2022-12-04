using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Controller : NetworkBehaviour
{
    public Text status;
    int identity;

    public override void OnStartServer()
    {
        if (isServer && isLocalPlayer)
        {
            Map.ClearAll();
            // set gameid in map
            gameObject.GetComponent<Database>().RequestMaxid();
            Debug.Log("Map Initialized!!");
        }
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;
            SendStartPosition(x, y);
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        status = GameObject.Find("PlayerStatus").GetComponent<Text>();
        RequestPlayerStatus();
    }

    void Update()
    {
        HandlePlayer();
    }

    [Command]
    void SendStartPosition(int x, int y)
    {
        int identity = Map.AddNewUser();
        SetIdentity(identity);
        Map.Visit(x, y, identity);
        Debug.Log("Start Position Updated!");
        Debug.Log(Map.PrintMap());
    }

    [TargetRpc]
    void SetIdentity(int identity)
    {
        this.identity = identity;
        Debug.Log("My Identity is " + this.identity);
    }

    [Client]
    void HandlePlayer()
    {
        if (isLocalPlayer)
        {
            int x = (int)transform.position.x;
            int y = (int)transform.position.y;

            if (Input.GetKeyDown(KeyCode.W))
            {
                RequestMoving(x, y, 0, 1, identity);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RequestMoving(x, y, 0, -1, identity);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                RequestMoving(x, y, -1, 0, identity);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                RequestMoving(x, y, 1, 0, identity);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                RequestAttack(x, y, 0, 1, identity);
                RequestPlayerStatus();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                RequestAttack(x, y, 0, -1, identity);
                RequestPlayerStatus();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RequestAttack(x, y, -1, 0, identity);
                RequestPlayerStatus();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                RequestAttack(x, y, 1, 0, identity);
                RequestPlayerStatus();
            }
        }
    }

    [Command]
    void RequestPlayerStatus()
    {
        string playerStat = Map.PrintPlayerStat();
        sendPlayerStatus(playerStat);
    }

    [ClientRpc]
    void sendPlayerStatus(string message)
    {
        status.text = message;
    }

    [Command]
    void RequestAttack(int x, int y, int dx, int dy, int subjectIdentity)
    {
        if (ValidateAttack(x, y, dx, dy))
        {
            int enemyIdentity = Map.GetPlayerIdentity(x + dx, y + dy);

            if (Map.Attack(subjectIdentity, enemyIdentity))
            {
                Debug.Log("Destroy! enemyIdentity: " + enemyIdentity);
                Map.Clear(x + dx, y + dy);

                NetworkConnectionToClient target = NetworkServer.connections[enemyIdentity - 1];
                enemyDestroy(target);
            }

            Debug.Log("Complete Attack Request to " + enemyIdentity);
            return;
        }
        Debug.Log("Deny Request Attack!!");
    }

    [TargetRpc]
    void enemyDestroy(NetworkConnection enemy)
    {
        enemy.Disconnect();
    }

    string DirectionToStringInMove(int dx, int dy)
    {
        if (dx == 0 && dy == 1)
        {
            return "mu";
        }
        if (dx == 0 && dy == -1)
        {
            return "md";
        }
        if (dx == -1 && dy == 0)
        {
            return "ml";
        }
        return "mr";
    }

    [Command]
    void RequestMoving(int x, int y, int dx, int dy, int identity)
    {
        if (ValidateMoving(x, y, dx, dy))
        {
            gameObject.GetComponent<Database>().RequestSave(Map.gameid, Map.mainturn, Map.subturn, identity, 0, DirectionToStringInMove(dx, dy), 0, x, y);
            Map.UpdateTurn();
            Debug.Log("Save Move Log in Database");

            Map.Clear(x, y);
            Map.Visit(x + dx, y + dy, identity);
            MovePlayer(dx, dy);
            Debug.Log("Complete Player Moving!!");
            Debug.Log(Map.PrintMap());
            return;
        }
        Debug.Log("Deny Request Moving!!");
    }

    [TargetRpc]
    void MovePlayer(int dx, int dy)
    {
        transform.position += new Vector3(dx, dy, 0);
        Debug.Log("Player moved!!");
    }

    [Server]
    bool ValidateMoving(int x, int y, int dx, int dy)
    {
        return Map.IsInRange(x + dx, y + dy) && !Map.IsPlayer(x + dx, y + dy);
    }

    [Server]
    bool ValidateAttack(int x, int y, int dx, int dy)
    {
        return Map.IsInRange(x + dx, y + dy) && Map.IsPlayer(x + dx, y + dy);
    }
}
