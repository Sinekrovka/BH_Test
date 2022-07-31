using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(order = 51, menuName = "Tools/SetColor", fileName = "Color Settings")]
public class ChangeColorCharacterSettings : ScriptableObject
{
    [SerializeField] private Color changedColor;
    [SerializeField] private float timeChangedColor;

    public float TimeChangedColor => timeChangedColor;
    public Color ChangedColor => changedColor;
}
