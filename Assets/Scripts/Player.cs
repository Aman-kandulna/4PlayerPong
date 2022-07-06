using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class Player : MonoBehaviourPunCallbacks
{
 
    public float speed;
    private Vector3 movementDirection;
    private PhotonView photonview;
    private SpriteRenderer spriteRenderer;
    private float score;
    private float Score
    {
        set
        {
            if (!GameManager.Instance.isTraining)
            {
                score = value;
                ScoreBoardUpdater.Instance.UpdateScore(photonview.CreatorActorNr, score);
            }
        }
        get
        {
            return score;
        }
    }
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        photonview = transform.parent.GetComponent<PhotonView>();
        movementDirection = Vector3.zero;
        Score = 0;
        SetPlayerColor();

       
    }

    private void SetPlayerColor()
    {
       Photon.Realtime.Player[] playerlist =  PhotonNetwork.PlayerList;
        for(int i = 0;i < playerlist.Length; i++)
        {
            Photon.Realtime.Player player = playerlist[i];
            
            if(photonView.CreatorActorNr == player.ActorNumber)
            {
                SetColor((int)player.CustomProperties["color"]);
            }
        }
    }

    private void SetColor(int colorcode)
    {
        switch (colorcode)
        {

            case 0:
                spriteRenderer.color = Color.red;
                break;
            case 1:
                spriteRenderer.color = Color.green;
                break;
            case 2:
                spriteRenderer.color = Color.yellow;
                break;
            case 3:
                spriteRenderer.color = Color.blue;
                break;
        }
    }
    
    void Update()
    {
        if (photonview.IsMine)
        {

            if (Input.GetKey(KeyCode.A))
            {
              
                movementDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                movementDirection = Vector2.right;
            }
            else
            {
                movementDirection = Vector2.zero;
            }
        }
        
        if(transform.localPosition.x <= -0.4  && movementDirection == Vector3.left)
        {
            movementDirection = Vector2.zero;
        }
        if(transform.localPosition.x >= 0.4 && movementDirection == Vector3.right)
        {
            movementDirection = Vector2.zero;
        }

    }

    [PunRPC]
    void UpdateScore()
    {
        Score++;
    }
    private void FixedUpdate()
    {
        
        transform.position += transform.TransformDirection(movementDirection) * speed * Time.fixedDeltaTime;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball") && PhotonNetwork.IsMasterClient)
        {
                photonView.RPC("UpdateScore", RpcTarget.All, null);
        }
    }
}


