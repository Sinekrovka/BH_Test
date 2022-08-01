using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class ChageColors : MonoBehaviour
{
    private int _points;
    private Material[] materials;
    [SerializeField] private ChangeColorCharacterSettings _dataColor;
    [SerializeField] private TextMeshPro _text;

    private void Awake()
    {
        _points = 0;
        materials = transform.GetComponentInChildren<Renderer>().materials;
        WinController.Instance.reloadRoom += NullPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform.parent)
        {
            if (other.transform.Find("Character").TryGetComponent(out ChageColors otherColors))
            {
                otherColors.ChangeColorThisObject();
                AddPoint();   
            }
        }
    }

    private void AddPoint()
    {
        _points++;
        _text.text = _points + "/3";
        if (_points >= 3)
        {
            WinController.Instance.WinGame(transform.parent.name);
        }
    }

    public void ChangeColorThisObject()
    {
        StartCoroutine(WaitTime());
    }

    private IEnumerator WaitTime()
    {
        transform.parent.gameObject.layer = 7;
        ChangeColor(_dataColor.ChangedColor);
        yield return new WaitForSeconds(_dataColor.TimeChangedColor);
        ChangeColor(Color.white);
        transform.parent.gameObject.layer = 6;
    }

    private void ChangeColor(Color color)
    {
        foreach (var material in materials)
        {
            material.color = color;
        }
    }

    private void NullPoints()
    {
        _points = 0;
        _text.text = _points + "/3";
    }
}
