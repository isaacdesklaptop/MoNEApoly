using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public GameObject createJoinCanvas;
    public GameObject roomCanvas;
    
    public TextMeshProUGUI roomTitle;
    
    public TMP_InputField createField;
    public TMP_InputField joinField;
    
    public TextMeshProUGUI readyButtonText;
    public Button readyButton;
    bool readyButtonColor = false;
    Color readyColor = new Color(0.5f,1,0.5f);
    Color unreadyColor = new Color(1, 0.5f, 0.5f);

    public ExitGames.Client.Photon.Hashtable photonPlayerProperties = new ExitGames.Client.Photon.Hashtable();
    bool playerReady = false;

    public Button startButton;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public Transform playerItemListing;

    void Start()
    {
        PhotonNetwork.JoinLobby();
        UpdateReady(readyButtonColor);
        roomCanvas.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    public void CreateRoom()
    {
        if (createField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(createField.text, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true });
        }
    }

    public void JoinRoom()
    {
        if (joinField.text.Length >= 1) 
        {
            PhotonNetwork.JoinRoom(joinField.text);
        }
    }

    public override void OnJoinedRoom()
    {
        createJoinCanvas.SetActive(false);
        roomCanvas.SetActive(true);
        roomTitle.text = $"Room Name: {PhotonNetwork.CurrentRoom.Name}";
        UpdatePlayerList();
    }

    void UpdateReady(bool currentState)
    {
        bool color = currentState;
        if (color)
        {
            readyButton.GetComponent<Image>().color = readyColor;
            readyButtonText.text = "Readied!";
        }
        else
        {
            readyButton.GetComponent<Image>().color = unreadyColor;
            readyButtonText.text = "Ready Up";
        }
        readyButtonColor = !readyButtonColor;
    }

    public void OnClickReady()
    {
        UpdateReady(readyButtonColor);
        playerReady = !playerReady;
        if (!photonPlayerProperties.ContainsKey("PlayerReady"))
        {
            photonPlayerProperties.Add("PlayerReady", playerReady);
        }
        photonPlayerProperties["PlayerReady"] = playerReady;
        PhotonNetwork.LocalPlayer.SetCustomProperties(photonPlayerProperties);
        UpdateStartButton(playerReady);
    }

    public void UpdateStartButton(bool ready)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (ready)
            {
                startButton.gameObject.SetActive(true);
            }
            else
            {
                startButton.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickStart()
    {
        int playerCount = PhotonNetwork.PlayerList.Length;
        Debug.Log(playerCount);
        int readyCount = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties.ContainsKey("PlayerReady"))
            {
                Debug.Log(player.CustomProperties["PlayerReady"]);
            }
            if ((bool)player.CustomProperties["PlayerReady"] == true)
            {
                readyCount++;
            }
        }
        if (!(readyCount == playerCount))
        {
            return;
        }
        PhotonNetwork.LoadLevel("MainGame");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerItemListing);
            newPlayerItem.SetPlayerInfo(player.Value);

            if (player.Value == PhotonNetwork.LocalPlayer)
            {
                newPlayerItem.ApplyLocalChanges();
            }

            playerItemsList.Add(newPlayerItem);
        }
    }
}
