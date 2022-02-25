using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneButton : MonoBehaviour
{
    [SerializeField] private Button straightButt;
    [SerializeField] private Button curvedButt;
    public bool status = false; // false - straight, true = curve
    private ColorBlock buttonColor;

    private void Start()
    {
        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        straightButt.colors = buttonColor;
    }

    public void ChangeButtonStraight()
    {
        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        straightButt.colors = buttonColor;

        status = false;

        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        curvedButt.colors = buttonColor;
    }

    public void ChangeButtonCurve()
    {
        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        curvedButt.colors = buttonColor;

        status = true;

        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        straightButt.colors = buttonColor;
    }
}

