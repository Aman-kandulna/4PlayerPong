using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviour, IPunObservable
{

    private Rigidbody2D rb;
    public float force;
    private Photon.Pun.PhotonView photonView;
    public static BallController Instance;
    private void Awake()
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
    void OnEnable()
    {

        photonView = GetComponent<Photon.Pun.PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(RandomizeDirection() * force,ForceMode2D.Impulse); // Randomize this.

    }

    Vector2 RandomizeDirection()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);

        return new Vector2(x, y).normalized;
    }
    public void ResetBallUtility()
    {
        photonView.RPC("ResetBallPosition", Photon.Pun.RpcTarget.All);

    }
    public void LaunchBallUtility()
    {
        photonView.RPC("LaunchBall", Photon.Pun.RpcTarget.All);
    }

    [Photon.Pun.PunRPC]
    private void ResetBallPosition()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    
    }
    [Photon.Pun.PunRPC]
    private void LaunchBall()
    {
        rb.AddForce(RandomizeDirection() * force, ForceMode2D.Impulse);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);
            stream.SendNext(rb.velocity);
        }
        else
        {
            rb.position = (Vector2)stream.ReceiveNext();
            rb.rotation = (float)stream.ReceiveNext();
            rb.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            rb.position += rb.velocity * lag;
        }
    }
}
