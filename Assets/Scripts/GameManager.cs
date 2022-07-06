using UnityEngine;
using Photon.Pun;


public class GameManager : MonoBehaviourPunCallbacks
{
    private int currentPlayerCount;
    public int maxPlayer;
    public GameObject StartingGameInCanvas;
    public GameObject GameMenuCanvas;

    [SerializeField]
    private Transform[] playerSpawnPositions;

    public static GameManager Instance;
    private int PlayerPosition;
   

    public bool isTraining;
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
        isTraining = true;
        currentPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
      
        
        if(currentPlayerCount == maxPlayer)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            photonView.RPC("TurnOffTrainingMode", RpcTarget.All, null);
            photonView.RPC("EnableStartingGameInCanvas", RpcTarget.All, null);
        
        }
        SetPlayerPosition();  
        SetUpCamera();

      
    }
    [PunRPC]
    public void EnableStartingGameInCanvas()
    {
        StartingGameInCanvas.SetActive(true);
    }
    private void SetPlayerPosition()
    {
        for (int i = 0; i < 4; i++)
        {
            if ((int)PhotonNetwork.CurrentRoom.CustomProperties["SpawnPosition" + i] == 0)
            {
                PhotonNetwork.Instantiate("Player", playerSpawnPositions[i].position, playerSpawnPositions[i].rotation);
                PlayerPosition = i;
                ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
                roomProperties["SpawnPosition" + i] = 1;
                roomProperties[PhotonNetwork.LocalPlayer.NickName] = i;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
                break;
            }
        }

    
    }

    private void SetUpCamera()
    {

        if (Camera.main != null)
        {
            Camera.main.transform.rotation = playerSpawnPositions[PlayerPosition].rotation;
            Camera.main.orthographicSize = 25;
            float rotation = Camera.main.transform.rotation.eulerAngles.z;
            switch (rotation)
            {
                case 0:
                    Camera.main.transform.position = new Vector3(0, -5, -10);
                    break;
                case -90:
                    Camera.main.transform.position = new Vector3(-5, 0, -10);
                    break;
                case 90:
                    Camera.main.transform.position = new Vector3(5, 0, -10);
                    break;
                case 180:
                    Camera.main.transform.position = new Vector3(0, 5, -10);
                    break;

            }
        }

    }
    [PunRPC]
    public void TurnOffTrainingMode()
    {
        isTraining = false;
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " left the room ");
        UpdateRoomSettings(otherPlayer);
    }

    public void OnClick_Menu()
    {
        EnableGameCanvasMenu();
    }
    private void EnableGameCanvasMenu()
    {
        GameMenuCanvas.SetActive(true);
    }
    public void OnClick_Continue()
    {
        DisableGameMenuCanvas();
    }
    private  void DisableGameMenuCanvas()
    {
        GameMenuCanvas.SetActive(false);
    }
   
    public void OnClick_LeaveRoom()
    { 
        PhotonNetwork.LeaveRoom();
    }
    
    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public void UpdateRoomSettings(Photon.Realtime.Player otherPlayer)
    {
        if (!PhotonNetwork.CurrentRoom.IsOpen)
            PhotonNetwork.CurrentRoom.IsOpen = true;
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        roomProperties["SpawnPosition" + PhotonNetwork.CurrentRoom.CustomProperties[otherPlayer.NickName]] = 0;
        roomProperties.Remove(otherPlayer.NickName);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }
    
}
