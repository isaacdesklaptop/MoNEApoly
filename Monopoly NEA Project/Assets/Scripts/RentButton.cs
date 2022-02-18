using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RentButton : MonoBehaviour
{
    public string localPlayerNick = PhotonNetwork.LocalPlayer.NickName;
    public PlayerEntity localPlayerEntity;

    public void OnClick()
    {
        localPlayerEntity = GameObject.Find("Player Object(Clone)").GetComponent<PlayerEntity>();
        localPlayerEntity.PayOtherPlayer(localPlayerEntity.currentPosition);
    }
}
