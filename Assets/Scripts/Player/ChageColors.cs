using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ChageColors : NetworkBehaviour
{
    private int _points;
    private Material[] materials;
    [SerializeField] private ChangeColorCharacterSettings _dataColor;

    private void Awake()
    {
        _points = 0;
        materials = GetComponentInChildren<Renderer>().materials;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ChageColors otherColors))
        {
            StartCoroutine(WaitTime());
            otherColors.AddPoint();
        }
    }

    public void AddPoint()
    {
        _points++;
        if (_points >= 3)
        {
            
        }
    }

    private IEnumerator WaitTime()
    {
        ChangeColor(_dataColor.ChangedColor);
        yield return new WaitForSeconds(_dataColor.TimeChangedColor);
        ChangeColor(Color.white);
    }

    private void ChangeColor(Color color)
    {
        foreach (var material in materials)
        {
            material.color = color;
        }
    }
}
