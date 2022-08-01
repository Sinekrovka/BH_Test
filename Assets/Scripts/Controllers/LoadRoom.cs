using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class LoadRoom : MonoBehaviour
{
    private void Awake()
    {
        NetworkManager.singleton.ServerChangeScene("SampleScene");
    }
}
