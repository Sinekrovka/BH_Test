using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 52, fileName = "Movement Params", menuName = "Tools/Movement Settings")]
public class MovementCharactersSettings : ScriptableObject
{
    [SerializeField] private float characterSpeed;
    [SerializeField] private float characterDashSpeed;
    [SerializeField] private float characterDistanceDash;

    public float Speed => characterSpeed;
    public float DashSpeed => characterDashSpeed;
    public float DistanceDash => characterDistanceDash;
}
