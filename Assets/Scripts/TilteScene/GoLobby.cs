using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoLobby : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    
}
