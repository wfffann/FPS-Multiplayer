using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


public class Launcher : MonoBehaviourPunCallbacks
{
    //组件
    public InputField roomName;
    public string playerPrefabName;

    //数值

    //Bool
    private bool connectedToMaster;
    private bool joinedRoom;

    /// <summary>
    /// 上线
    /// </summary>
    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "Alpha";
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    public void CreateRoom()
    {
        if (!connectedToMaster || joinedRoom) return;

        PhotonNetwork.CreateRoom(roomName.text, 
            new RoomOptions() { MaxPlayers = 16 }, 
            TypedLobby.Default);
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    public void JoinRoom()
    {
        if (!connectedToMaster || joinedRoom) return;

        PhotonNetwork.JoinRoom(roomName.text);
    }

    /// <summary>
    /// 上线后的回调
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        connectedToMaster = true;
    }

    /// <summary>
    /// 创建房间后的回调
    /// </summary>
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        joinedRoom = true;
    }

    /// <summary>
    /// 加入房间的回调(创建房间后也会自动加入房间
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined Room");

        PhotonNetwork.Instantiate(playerPrefabName, Vector3.zero, Quaternion.identity);
    }
}