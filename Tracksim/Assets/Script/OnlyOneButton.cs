// (BPC-PRP project) creating Track for robot
// author: Petr Šopák (221022)
// team: team 3
// class function: UI visual features + getters

using UnityEngine;
using UnityEngine.UI;

public class OnlyOneButton : MonoBehaviour
{
    [SerializeField] private Button straightButt;
    [SerializeField] private Button curvedButt;
    [SerializeField] private Button triButt;

    private ColorBlock buttonColor;

    public CreateLine.Status typeLine; // type of line

    /// <summary>
    /// inicialization in first frame of game
    /// </summary>
    private void Start()
    {
        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        straightButt.colors = buttonColor;
    }

    /// <summary>
    /// get the Line function and cancel every other functions + added visual features
    /// </summary>
    public void ChangeButtonStraight()
    {
        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        straightButt.colors = buttonColor;

        typeLine = CreateLine.Status.straight;

        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        curvedButt.colors = buttonColor;

        buttonColor = triButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        triButt.colors = buttonColor;
    }

    /// <summary>
    /// get the Line function and cancel every other functions + added visual features
    /// </summary>
    public void ChangeButtonCurve()
    {
        buttonColor = curvedButt.colors;
        buttonColor.normalColor = new Color32(255, 128, 128, 255);
        curvedButt.colors = buttonColor;

        typeLine = CreateLine.Status.curve;

        buttonColor = straightButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        straightButt.colors = buttonColor;

        buttonColor = triButt.colors;
        buttonColor.normalColor = new Color32(255, 255, 255, 255);
        triButt.colors = buttonColor;
    }

    /// <summary>
    /// get the Line function and cancel every other functions + added visual features
    /// </summary>
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
    }
}

