using System;
using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;

public class WinController : MonoBehaviour
{
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private TextMeshProUGUI _winText;

    public Action reloadRoom;
    
    public static WinController Instance;

    private void Awake()
    {
        Instance = this;
    }
    
    public void WinGame(string name)
    {
        _winCanvas.SetActive(true);
        _winText.text = "Winned player: " + name;
        StartCoroutine(RestartGame());
    }
    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(5);
        reloadRoom?.Invoke();
        NetworkManager.singleton.ServerChangeScene("LoadScene");
        
    }
}
