using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyOneButton : MonoBehaviour
{
    [SerializeField] private Button straightButt;
    [SerializeField] private Button curvedButt;
    [SerializeField] private Button triButt;
    [SerializeField] private Button newLineButt;
    private ColorBlock buttonColor;

    public CreateLine.Status typeLine; // type of line

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

        typeLine = CreateLine.Status.straight;

        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        curvedButt.colors = buttonColor;

        buttonColor = newLineButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        newLineButt.colors = buttonColor;

        buttonColor = triButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        triButt.colors = buttonColor;
    }

    public void ChangeButtonCurve()
    {
        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        curvedButt.colors = buttonColor;

        typeLine = CreateLine.Status.curve;

        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        straightButt.colors = buttonColor;

        buttonColor = newLineButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        newLineButt.colors = buttonColor;

        buttonColor = triButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        triButt.colors = buttonColor;
    }

    public void ChangeButtonTri()
    {
        buttonColor = triButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        triButt.colors = buttonColor;

        typeLine = CreateLine.Status.tri;

        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        straightButt.colors = buttonColor;

        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        curvedButt.colors = buttonColor;

        buttonColor = newLineButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        newLineButt.colors = buttonColor;
    }

    public void ChangeButtonNewLine()
    {
        buttonColor = newLineButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        newLineButt.colors = buttonColor;

        typeLine = CreateLine.Status.newLine;

        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        straightButt.colors = buttonColor;

        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        curvedButt.colors = buttonColor;

        buttonColor = triButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        triButt.colors = buttonColor;
    }
}

