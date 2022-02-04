using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameManager GameManager;
    public Vector3 playerPosition;

    private void Start()
    {
        playerPosition = GameManager.propertiesArray[0].transform.position;
        PhotonNetwork.Instantiate(playerPrefab.name, playerPosition, Quaternion.identity);
    }
}
