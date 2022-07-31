using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinController : MonoBehaviour
{
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private TextMeshProUGUI _winText;
    
    public static WinController Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void WinGame(string name)
    {
        _winCanvas.SetActive(true);
        _winText.text = "Winned player: " + name;
    }

    public void RestartGame()
    {
        
    }
}
