// (BPC-PRP project) creating Track for robot
// author: Petr Šopák (221022)
// team: team 3
// class funtion: getting informations from user choice (for triLine feature)

using UnityEngine;
using UnityEngine.UI;

public class DropDownInput : MonoBehaviour
{
    [SerializeField] private Dropdown dropDownShape;
    [SerializeField] private Dropdown dropDownType;
    [SerializeField] private CreateLine line;

    /// <summary>
    /// get and set dropdown input
    /// </summary>
    public void DDShapeInput()
    {
        switch (dropDownShape.value)
        {
            case 0: // straight
                line.TriLineShape = 0;
                break;

            case 1: // curve
                line.TriLineShape = 1;
                break;

            default: // straight
                line.TriLineShape = 0;
                break;
        }
    }

    /// <summary>
    /// get and set dropdown input
    /// </summary>
    public void DDTypeInput()
    {
        switch (dropDownType.value)
        {
            case 0: // one Side - left
                line.TriLineType = 0;
                break;

            case 1: // one Side - right
                line.TriLineType = 1;
                break;

            case 2: // one Side - right
                line.TriLineType = 2;
                break;

            default: // one Side - left
                line.TriLineType = 0;
                break;
        }
    }
}
