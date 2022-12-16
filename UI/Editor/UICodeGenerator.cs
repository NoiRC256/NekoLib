using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core;
using System.Runtime.ExceptionServices;

namespace NekoSystem.UI.Editor
{
    public class UICodeGenerator
    {

        public class UIViewElement
        {
            public string Name;
            public string Path;
            public string ComponentName;
            public UIViewElement(string name, string path, string componentName)
            {
                Name = name;
                Path = path;
                ComponentName = componentName;
            }

            public override string ToString()
            {
                string str = string.Format("Name={0} || Path={1} || ComponentName={2}", Name, Path, ComponentName);
                return str;
            }
        }

        [MenuItem("Assets/Generate UI Code")]
        public static void GenerateCode()
        {
            CreatePath();
        }

        public static void CreatePath()
        {
            Object[] selectedObjects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets | SelectionMode.TopLevel);
            if (selectedObjects.Length == 0) return;
            GameObject obj = (GameObject)selectedObjects[0];
            //Recursively record UI view elements in the gameobject hierarchy.

        }

        /// <summary>
        /// Generate a IU view class and save the file at the specified path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="className"></param>
        /// <param name="elements"></param>
        public static void GenerateView(string path, string className, List<UIViewElement> elements)
        {
            var sw = new StreamWriter(path, false, Encoding.UTF8);
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("using UnityEngine;");
            strBuilder.AppendLine("using UnityEngine.UI;");
            strBuilder.AppendLine();
            strBuilder.AppendFormat("public class {0} : UIViewBase", className);
            strBuilder.AppendLine();
            strBuilder.AppendLine("{");
            //Generate component properties.
            foreach (UIViewElement element in elements)
            {
                strBuilder.AppendFormat("\t public {0} {1} {get; private set;} ", element.ComponentName, element.Name);
            }
            strBuilder.AppendLine("}");
            sw.Write(strBuilder);
            sw.Flush();
            sw.Close();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}