using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Controller : NetworkBehaviour
{
    public override void OnStartServer()
    {
        if (isServer && isLocalPlayer)
        {
            Map.ClearAll();
            Debug.Log("Map Initialized!!");
        }

        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        Map.Visit(x, y);
        Debug.Log("Start Position Updated!");
        Debug.Log(Map.PrintMap());
    }

    void Update()
    {
        HandlePlayer();
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
                RequestMoving(x, y, 0, 1);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                RequestMoving(x, y, 0, -1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                RequestMoving(x, y, -1, 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                RequestMoving(x, y, 1, 0);
            }
        }
    }

    [Command]
    void RequestMoving(int x, int y, int dx, int dy)
    {
        if (ValidateMoving(x, y, dx, dy))
        {
            Map.Clear(x, y);
            Map.Visit(x + dx, y + dy);
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
}
