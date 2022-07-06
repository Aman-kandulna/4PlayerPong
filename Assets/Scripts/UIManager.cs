using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIManager : MonoBehaviour
{

    public GameObject ConnectingToServerUI;
    public GameObject MainMenuUI;
    public GameObject charactercustomisationUI;
    public GameObject MessageUI;
    public TMP_Text message;
    public static UIManager Instance;

    public void Awake()
    {
        if(Instance != null && Instance !=this)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnClick_Exit()
    {
        Application.Quit();
    }
    private void OnClick_CharacterCustomisation()
    {
        MainMenuUI.SetActive(false);
        charactercustomisationUI.SetActive(true);
    }
    private void OnClick_Done()
    {
        charactercustomisationUI.SetActive(false );
        MainMenuUI.SetActive(true );
       
    }
    public void EnableMessage(string _message)
    {
        message.text = _message;
        MessageUI.SetActive(true);
    }
    private void OnClick_Close()
    {
        MessageUI.SetActive(false);
    }
    public void EnableMainMenu()
    {
        ConnectingToServerUI.SetActive(false);
        MainMenuUI.SetActive(true);

    }

}
