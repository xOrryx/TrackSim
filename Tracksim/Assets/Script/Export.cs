// (BPC-PRP project) creating Track for robot
// author: Petr Šopák (221022)
// team: team 3
// class function: exporting lines to .yaml format
// used library from: https://github.com/SrejonKhan/AnotherFileBrowser

using UnityEngine;
using System.IO;
using System.Globalization;

// source: https://github.com/SrejonKhan/AnotherFileBrowser
using AnotherFileBrowser.Windows;

public class Export : MonoBehaviour
{
    [SerializeField] private CreateLine line;

    private string path;

    /// <summary>
    /// Get path to export
    /// </summary>
    public void ExportModel()
    {
        #if UNITY_STANDALONE_WIN

        var bp = new BrowserProperties();
        bp.filter = "yaml files (*.yaml)|*.yaml";
        bp.filterIndex = 0;

        new FileBrowser().SaveFileBrowser(bp, "FileName", ".yaml", tempPath =>
        {
            path = tempPath;
        });
        #endif

        if (path != null)
            Measure_Export(path);
        else
            return;
    }

    /// <summary>
    /// creating file with main Line points
    /// </summary>
    /// <param name="pathToFile"></param>
    private void Measure_Export(string pathToFile)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("lines_segments_array:");
        writer.WriteLine();

        for (int i = 0; i < line.points.Count - 1; i++)
        {
            string x1 = "F4";
            string x2 = "F4";
            string y1 = "F4";
            string y2 = "F4";

            string mezera1 = " ";
            string mezera2 = " ";
            string mezera3 = " ";
            string mezera4 = " ";

            string parita1 = " ";
            string parita2 = " ";
            string parita3 = " ";
            string parita4 = " ";


            if (line.points[i].x % 1 == 0)
            {
                x1 = "F1";
                mezera1 = "    ";
            }
            if (line.points[i + 1].x % 1 == 0)
            {
                x2 = "F1";
                mezera2 = "    ";
            }
            if (line.points[i].z % 1 == 0)
            {
                y1 = "F1";
                mezera3 = "    ";
            }
            if (line.points[i + 1].z % 1 == 0)
            {
                y2 = "F1";
                mezera4 = "    ";
            }



            if (Mathf.Sign(line.points[i].x) == -1)
            {
                parita1 = "";
            }
            if (Mathf.Sign(line.points[i].z) == -1)
            {
                parita2 = "";
            }
            if (Mathf.Sign(line.points[i + 1].x) == -1)
            {
                parita3 = "";
            }
            if (Mathf.Sign(line.points[i + 1].z) == -1)
            {
                parita4 = "";
            }

            writer.WriteLine("  - [" + parita1 + (line.points[i].x / 100).ToString(x1, CultureInfo.InvariantCulture) + "," + mezera1 + parita2 + (line.points[i].z / 100).ToString(y1, CultureInfo.InvariantCulture) + "," + mezera3 + parita3 + (line.points[i + 1].x / 100).ToString(x2, CultureInfo.InvariantCulture) + "," + mezera2 + parita4 + (line.points[i + 1].z / 100).ToString(y2, CultureInfo.InvariantCulture) + "]");
        }

        if (line.usedTri)
        {
            WriteTriLines(writer);
        }

        writer.WriteLine();
        writer.WriteLine("line_width: 0.02");
        writer.Close();
    }

    /// <summary>
    /// if Tri Line feature is used, the new lines will be exported
    /// </summary>
    /// <param name="writer"></param>
    private void WriteTriLines(StreamWriter writer)
    {
        for (int i = 0; i < line.triPoints.Count - 1; i+=2)
        {
            string x1 = "F4";
            string x2 = "F4";
            string y1 = "F4";
            string y2 = "F4";

            string mezera1 = " ";
            string mezera2 = " ";
            string mezera3 = " ";
            string mezera4 = " ";

            string parita1 = " ";
            string parita2 = " ";
            string parita3 = " ";
            string parita4 = " ";


            if (line.triPoints[i].x % 1 == 0)
            {
                x1 = "F1";
                mezera1 = "    ";
            }
            if (line.triPoints[i + 1].x % 1 == 0)
            {
                x2 = "F1";
                mezera2 = "    ";
            }
            if (line.triPoints[i].z % 1 == 0)
            {
                y1 = "F1";
                mezera3 = "    ";
            }
            if (line.triPoints[i + 1].z % 1 == 0)
            {
                y2 = "F1";
                mezera4 = "    ";
            }



            if (Mathf.Sign(line.triPoints[i].x) == -1)
            {
                parita1 = "";
            }
            if (Mathf.Sign(line.triPoints[i].z) == -1)
            {
                parita2 = "";
            }
            if (Mathf.Sign(line.triPoints[i + 1].x) == -1)
            {
                parita3 = "";
            }
            if (Mathf.Sign(line.triPoints[i + 1].z) == -1)
            {
                parita4 = "";
            }

            writer.WriteLine("  - [" + parita1 + (line.triPoints[i].x / 100).ToString(x1, CultureInfo.InvariantCulture) + "," + mezera1 + parita2 + (line.triPoints[i].z / 100).ToString(y1, CultureInfo.InvariantCulture) + "," + mezera3 + parita3 + (line.triPoints[i + 1].x / 100).ToString(x2, CultureInfo.InvariantCulture) + "," + mezera2 + parita4 + (line.triPoints[i + 1].z / 100).ToString(y2, CultureInfo.InvariantCulture) + "]");
        }
    }

}
