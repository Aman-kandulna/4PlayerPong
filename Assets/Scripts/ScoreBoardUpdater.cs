using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
public class ScoreBoardUpdater : MonoBehaviourPunCallbacks
{
    public TMP_Text[] namelist;
    public static ScoreBoardUpdater Instance;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
        else
        {
            Instance = this;
        }
    }
    public void Start()
    {
        UpdatePlayerNames();
    }
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        UpdatePlayerNames();
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdatePlayerNames();
        
    }
    private void UpdatePlayerNames()
    {
        
       for(int i = 0; i < 4 ; i++)
        {
            if(i < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                namelist[i].text = PhotonNetwork.PlayerList[i].NickName + "\n" + "0";
            }
            
            else
            {
                namelist[i].text = "-----------" + "\n" + "0";
            }
          
        }
    }
    public void UpdateScore(int playerActorNo, float score)
    {
        for (int i = 0; i < 4; i++)
        {
            if (i < PhotonNetwork.CurrentRoom.PlayerCount)
            {
                if(playerActorNo == PhotonNetwork.PlayerList[i].ActorNumber)
                {
                    namelist[i].text = PhotonNetwork.PlayerList[i].NickName + "\n" + score.ToString();
                }
            }

            else
            {
                namelist[i].text = "-----------" + "\n" + "0";
            }

        }
    }
}
