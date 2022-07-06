using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public TMPro.TMP_Text timer;
    public int countDownMaxTime;
    public int countDownTime;
 
    private void OnEnable()
    {
        BallController.Instance.ResetBallUtility();
        countDownTime = countDownMaxTime;
        StartCoroutine(Timer());
    }
   
    IEnumerator Timer()
    {
        while(countDownTime>=0)
        {

            timer.text = timer.text = countDownTime.ToString();
            countDownTime--;
            yield return new WaitForSeconds(1);

        }
        OnCountDownFinish();
    }
    void OnCountDownFinish()
    {
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            BallController.Instance.LaunchBallUtility();
        }
        this.gameObject.SetActive(false);
    }
    
}
