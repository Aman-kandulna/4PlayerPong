using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
public enum RoomStatus
{
    ROOM_FULL,
    ROOM_DOES_NOT_EXISTS,
    ROOM_OPEN_TO_JOIN,

}

public class LobbyManagerScript : MonoBehaviourPunCallbacks
{
    public TMPro.TMP_InputField roomName;
    public TMPro.TMP_InputField playerName;
    public TMPro.TMP_Dropdown colorsDropDown;
    
    private List<RoomInfo> _roomList = new List<RoomInfo>();
    ExitGames.Client.Photon.Hashtable color = new ExitGames.Client.Photon.Hashtable();
    ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
    RoomOptions roomOption = new RoomOptions();

    public void Start()
    {
        setDefaultPlayerSettings();
        Connect();

        for (int i = 0; i < 4; i++)
        {
            roomProperties["SpawnPosition" + i] = 0;
        }
      

    }
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }
   
   
    public void Onclick_CreateRoom()
    {
        
        if (!string.IsNullOrEmpty(roomName.text))
        { 
            switch (CheckRoomStatus(roomName.text))
            {
                case RoomStatus.ROOM_DOES_NOT_EXISTS:
                    SetNickName();
                    roomOption.CustomRoomProperties = roomProperties;
                    PhotonNetwork.CreateRoom(roomName.text,roomOption);
                    break;
                case RoomStatus.ROOM_OPEN_TO_JOIN:
                case RoomStatus.ROOM_FULL:
                    UIManager.Instance.EnableMessage("Room already exists");
                    break;

            }
        }
        else
        {
            UIManager.Instance.EnableMessage("RoomName cannot be empty");
        }

    }

    public void Onclick_JoinRoom()
    {
        if (!string.IsNullOrEmpty(roomName.text))
        {
           switch(CheckRoomStatus(roomName.text))
            {
                case RoomStatus.ROOM_DOES_NOT_EXISTS:
                    UIManager.Instance.EnableMessage("Room does not exits");
                    break;
                case RoomStatus.ROOM_OPEN_TO_JOIN:
                    SetNickName();
                    PhotonNetwork.JoinRoom(roomName.text);
                    break;
                case RoomStatus.ROOM_FULL:
                    UIManager.Instance.EnableMessage("Room is full");
                    break;
                
            }
        }
        else
        {
            UIManager.Instance.EnableMessage("RoomName cannot be empty");
        }
    }
    public void setDefaultPlayerSettings()
    {
        color["color"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(color);
    
    }

    public  void  SetColorToPlayer()
    {
        color["color"] = colorsDropDown.value;
        PhotonNetwork.LocalPlayer.SetCustomProperties(color);
    
     
    }
    private void SetNickName()
    {
        if (string.IsNullOrEmpty(playerName.text))
        {
            PhotonNetwork.LocalPlayer.NickName = "Player#" + UnityEngine.Random.Range(0, 1000);
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = playerName.text;
        }
    }
        public  RoomStatus CheckRoomStatus(string roomName)
    {
        RoomInfo t = null;
        RoomStatus roomStatus;
        
        foreach (RoomInfo room in _roomList)
        {
            if (roomName == room.Name)
            {
                t = room;
            }
            
        }
        
        if(t == null)
        {
            roomStatus = RoomStatus.ROOM_DOES_NOT_EXISTS;
        }
        else if(!t.IsOpen)
        {
            roomStatus = RoomStatus.ROOM_FULL;
        }
        else
        {
            roomStatus = RoomStatus.ROOM_OPEN_TO_JOIN;
        }

        return roomStatus;
        
    }
 
    #region overriddenPhotonNetworkFunctions

   

    public override void OnConnectedToMaster()
    {
        
        PhotonNetwork.JoinLobby();
       
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("GameLevel1");
    }
    public override void OnJoinedLobby()
    {
        UIManager.Instance.EnableMainMenu();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomList = roomList;
  
    }

    #endregion

}