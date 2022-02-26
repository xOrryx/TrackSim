using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

// source: https://github.com/SrejonKhan/AnotherFileBrowser
using AnotherFileBrowser.Windows;

public class Export : MonoBehaviour
{
    [SerializeField] private CreateLine line;

    private string path;


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

    private void Measure_Export(string pathToFile)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("lines_segments_array:");
        writer.WriteLine();

        for(int i = 0; i < line.points.Count-1; i++)
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
            if (line.points[i+1].x % 1 == 0)
            {
                x2 = "F1";
                mezera2 = "    ";
            }
            if (line.points[i].z % 1 == 0)
            {
                y1 = "F1";
                mezera3 = "    ";
            }
            if (line.points[i+1].z % 1 == 0)
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
            if (Mathf.Sign(line.points[i+1].x) == -1)
            {
                parita3 = "";
            }
            if (Mathf.Sign(line.points[i+1].z) == -1)
            {
                parita4 = "";
            }

            writer.WriteLine("  - [" + parita1 + line.points[i].x.ToString(x1, CultureInfo.InvariantCulture) + "," + mezera1 + parita2 + line.points[i].z.ToString(y1, CultureInfo.InvariantCulture) + "," + mezera2 + parita3 + line.points[i + 1].x.ToString(x2, CultureInfo.InvariantCulture) + "," + mezera3 + parita4 + line.points[i + 1].z.ToString(y2, CultureInfo.InvariantCulture) + "]");
        }

        writer.WriteLine();
        writer.WriteLine("line_width: 0.02");
        writer.Close();
    }

}
