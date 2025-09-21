#if UNITY_EDITOR

#if JsonLoader
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class SheetParsing : EditorWindow
{
    string urlKey = "GoogleSheet";
    string functionKey = "GoogleFunction";
    string sheetUrl = "";
    string functionUrl = "";
    string gid;
    string jsonFileName;

    const int keyIndex = 0;
    const int typeIndex = 1;
    const int width = 250;

    [MenuItem("Tools/ GoogleSheetParsing")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(SheetParsing));
        window.maxSize = window.minSize = new Vector2(700, 300);
    }

    private void OnGUI()
    {
        var btnOptions = new[] { GUILayout.Width(128), GUILayout.Height(22) };

        if (!PlayerPrefs.HasKey(urlKey) || !PlayerPrefs.HasKey(functionKey))
        {
            if (!PlayerPrefs.HasKey(urlKey))
            {
                sheetUrl = EditorGUILayout.TextField("SheetUrl", sheetUrl);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("SheetUrl : /edit 이전까지 값", GUILayout.Width(200));

                if (GUILayout.Button("Save", btnOptions))
                {
                    PlayerPrefs.SetString(urlKey, sheetUrl);
                }
                EditorGUILayout.EndHorizontal();
                GUIStyle whiteLine = new GUIStyle();
                whiteLine.normal.background = Texture2D.grayTexture; // 하얀색 텍스처
                whiteLine.margin = new RectOffset(0, 0, 4, 4); // 위아래 여백
                whiteLine.fixedHeight = 2; // 선 두께
                GUILayout.Box("", whiteLine, GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }

            if (!PlayerPrefs.HasKey(functionKey))
            {
                functionUrl = EditorGUILayout.TextField("FunctionUrl", functionUrl);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("FunctionUrl : Apps Script Url", GUILayout.Width(200));

                if (GUILayout.Button("Save", btnOptions))
                {
                    PlayerPrefs.SetString(functionKey, functionUrl);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            sheetUrl = PlayerPrefs.GetString(urlKey,"");
            EditorGUILayout.LabelField("URL : " + sheetUrl);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("구글 시트 저장 삭제",GUILayout.Width(width));
            if (GUILayout.Button("Delete", btnOptions))
            {
                PlayerPrefs.DeleteKey(urlKey);
            }
            EditorGUILayout.EndHorizontal();
            
            GUIStyle whiteLine = new GUIStyle();
            whiteLine.normal.background = Texture2D.grayTexture; // 하얀색 텍스처
            whiteLine.margin = new RectOffset(0, 0, 4, 4); // 위아래 여백
            whiteLine.fixedHeight = 2; // 선 두께
            GUILayout.Box("", whiteLine, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            gid = EditorGUILayout.TextField("Gid", gid);
            EditorGUILayout.LabelField("GoogleGid : 원하는 시트의 맨뒤 아이디 값");
            jsonFileName = EditorGUILayout.TextField("SaveFileName", jsonFileName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("SaveFileName : 저장 할 Json 파일 이름", GUILayout.Width(width));
            if (GUILayout.Button("Create", btnOptions))
            {
                Parsing();
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Box("", whiteLine, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            functionUrl = PlayerPrefs.GetString(functionKey,"");
            EditorGUILayout.LabelField("Apps Script : " + functionUrl);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Apps Script 삭제", GUILayout.Width(width));
            if (GUILayout.Button("Delete", btnOptions))
            {
                PlayerPrefs.DeleteKey(functionKey);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("모든 Table 불러오기", GUILayout.Width(width));
            if (GUILayout.Button("AllParsing", btnOptions))
            {
                AllParsing();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Box("", whiteLine, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Apps Script,Sheet Url 삭제", GUILayout.Width(width));

            if (GUILayout.Button("Delete", btnOptions))
            {
                PlayerPrefs.DeleteKey(urlKey);
                PlayerPrefs.DeleteKey(functionKey);
            }

            EditorGUILayout.EndHorizontal();
        }

    }

    void Parsing()
    {
        EditorCoroutineUtility.StartCoroutine(GoogleSheetParsing(), this);
    }

    void AllParsing()
    {
        EditorCoroutineUtility.StartCoroutine(GoogleSheetAllParsing(), this);
    }

    IEnumerator GoogleSheetParsing()
    {
        string _url = string.Format("{0}/export?format=tsv&gid={1}", sheetUrl, gid);
        string data = string.Empty;
        string fileName = jsonFileName.Replace("$", "");
        fileName = (fileName == "" ? "JsonFile" : fileName);

        UnityWebRequest request = UnityWebRequest.Get(_url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            EditorUtility.DisplayDialog("Fail", "GoogleConnect Fail!\n" + request.error, "OK");
            yield break;
        }

        data = request.downloadHandler.text;

        List<string> strs = data.Split("\r\n").ToList();
        List<string> keys = strs[keyIndex].Split('\t').ToList();
        List<string> types = strs[typeIndex].Split('\t').ToList();
        JArray jArray = new JArray();

        for (int row = 2; row < strs.Count; row++)
        {
            JObject keyValuePairs = new JObject();
            List<string> datas = strs[row].Split("\t").ToList();

            if (datas[0].Equals("DB_IGNORE") || datas[1].Equals("")) continue;

            for (int column = 1; column < keys.Count; column++)
            {
                if (types[column].Equals("DB_IGNORE") || keys[column].Equals(""))
                {
                    continue;
                }

                switch (types[column])
                {
                    case "string":
                        keyValuePairs.Add(keys[column], datas[column].Equals("") ? "" : datas[column]);
                        break;
                    case "int":
                        int intValue = 0;

                        if (!datas[column].Equals(""))
                        {
                            int.TryParse(datas[column], out intValue);
                        }

                        keyValuePairs.Add(keys[column], intValue);
                        break;
                    case "float":
                        float floatValue = 0;

                        if (!datas[column].Equals(""))
                        {
                            float.TryParse(datas[column], out floatValue);
                        }

                        keyValuePairs.Add(keys[column], floatValue);
                        break;
                    case "arrayFloat":
                        JArray jArray1 = new JArray();
                        List<float> nums = new List<float>();
                        List<string> str_nums = datas[column].Split(",").ToList();

                        for (int k = 0; k < str_nums.Count; k++)
                        {
                            if (str_nums[k].Equals("")) continue;
                            float fValue = 0;
                            float.TryParse(str_nums[k], out fValue);
                            nums.Add(fValue);
                        }

                        jArray1.Add(nums);
                        keyValuePairs.Add(keys[column], jArray1);
                        break;
                    case "arrayString":
                        JArray jArray2 = new JArray();
                        List<string> strValues = datas[column].Split(",").ToList();
                        if (strValues.Count == 1 && strValues[0].Equals("")) { strValues.Clear(); }
                        jArray2.Add(strValues);
                        keyValuePairs.Add(keys[column], jArray2);
                        break;
                    case "arrayInt":
                        JArray jArray3 = new JArray();
                        List<int> nums2 = new List<int>();
                        List<string> str_nums2 = datas[column].Split(",").ToList();

                        for (int k = 0; k < str_nums2.Count; k++)
                        {
                            if (str_nums2[k].Equals("")) continue;
                            int iValue = 0;
                            int.TryParse(str_nums2[k], out iValue);
                            nums2.Add(iValue);
                        }

                        jArray3.Add(nums2);
                        keyValuePairs.Add(keys[column], jArray3);
                        break;
                    case "long":
                        long longValue = 0;

                        if (!datas[column].Equals(""))
                        {
                            long.TryParse(datas[column], out longValue);
                        }

                        keyValuePairs.Add(keys[column], longValue);
                        break;
                    case "byte":
                        byte byteValue = 0;

                        if (!datas[column].Equals(""))
                        {
                            byte.TryParse(datas[column], out byteValue);
                        }

                        keyValuePairs.Add(keys[column], byteValue);
                        break;
                    case "bool":
                        bool boolNum = false;

                        if (!datas[column].Equals(""))
                        {
                            if (datas[column].Equals("TRUE"))
                            {
                                boolNum = true;
                            }
                            else if (datas[column].Equals("FALSE"))
                            {
                                boolNum = false;
                            }
                        }

                        keyValuePairs.Add(keys[column], boolNum);
                        break;
                    case "Object":
                        //개발중 
                        break;
                    default:
                        break;
                }
            }

            jArray.Add(keyValuePairs);
        }

        string path = "";
        path = Path.Combine(Application.dataPath + "/JsonFiles/", fileName + ".json");

        Debug.Log(jsonFileName + " Create");

        File.WriteAllText(path, jArray.ToString());

        EditorUtility.DisplayDialog("Success", "Json successfully Save!", "OK");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    IEnumerator GoogleSheetAllParsing()
    {
        functionUrl = PlayerPrefs.GetString(functionKey);
        UnityWebRequest web = UnityWebRequest.Get(functionUrl);
        yield return web.SendWebRequest();

        if (web.result == UnityWebRequest.Result.Success)
        {
            // 성공적으로 응답을 받았을 때
            string[] strings = web.downloadHandler.text.Split(",");

            for (int i = 0; i < strings.Length; i++)
            {
                string[] nameId = strings[i].Split("/");
                string _url = string.Format("{0}/export?format=tsv&gid={1}", sheetUrl, nameId[1]);
                string data = string.Empty;
                string fileName = nameId[0];

                if (fileName.Contains("&"))
                {
                    continue;
                }

                UnityWebRequest request = UnityWebRequest.Get(_url);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    EditorUtility.DisplayDialog("Fail", "GoogleConnect Fail!", "OK");
                    yield break;
                }

                data = request.downloadHandler.text;

                Debug.Log(data);

                List<string> strs = data.Split("\r\n").ToList();
                List<string> keys = strs[keyIndex].Split('\t').ToList();
                List<string> types = strs[typeIndex].Split('\t').ToList();
                JArray jArray = new JArray();

                for (int row = 2; row < strs.Count; row++)
                {
                    JObject keyValuePairs = new JObject();
                    List<string> datas = strs[row].Split("\t").ToList();

                    if (datas[0].Equals("DB_IGNORE") || datas[1].Equals("")) continue;

                    for (int column = 1; column < keys.Count; column++)
                    {
                        if (types[column].Equals("DB_IGNORE") || keys[column].Equals(""))
                        {
                            continue;
                        }

                        switch (types[column])
                        {
                            case "string":
                                keyValuePairs.Add(keys[column], datas[column].Equals("") ? "" : datas[column]);
                                break;
                            case "int":
                                int intValue = 0;

                                if (!datas[column].Equals(""))
                                {
                                    int.TryParse(datas[column], out intValue);
                                }

                                keyValuePairs.Add(keys[column], intValue);
                                break;
                            case "float":
                                float floatValue = 0;

                                if (!datas[column].Equals(""))
                                {
                                    float.TryParse(datas[column], out floatValue);
                                }

                                keyValuePairs.Add(keys[column], floatValue);
                                break;
                            case "arrayFloat":
                                JArray jArray1 = new JArray();
                                List<float> nums = new List<float>();
                                List<string> str_nums = datas[column].Split(",").ToList();

                                for (int k = 0; k < str_nums.Count; k++)
                                {
                                    if (str_nums[k].Equals("")) continue;
                                    float fValue = 0;
                                    float.TryParse(str_nums[k], out fValue);
                                    nums.Add(fValue);
                                }

                                jArray1.Add(nums);
                                keyValuePairs.Add(keys[column], jArray1);
                                break;
                            case "arrayString":
                                JArray jArray2 = new JArray();
                                List<string> strValues = datas[column].Split(",").ToList();
                                if (strValues.Count == 1 && strValues[0].Equals("")) { strValues.Clear(); }
                                jArray2.Add(strValues);
                                keyValuePairs.Add(keys[column], jArray2);
                                break;
                            case "arrayInt":
                                JArray jArray3 = new JArray();
                                List<int> nums2 = new List<int>();
                                List<string> str_nums2 = datas[column].Split(",").ToList();

                                for (int k = 0; k < str_nums2.Count; k++)
                                {
                                    if (str_nums2[k].Equals("")) continue;
                                    int iValue = 0;
                                    int.TryParse(str_nums2[k], out iValue);
                                    nums2.Add(iValue);
                                }

                                jArray3.Add(nums2);
                                keyValuePairs.Add(keys[column], jArray3);
                                break;
                            case "long":
                                long longValue = 0;

                                if (!datas[column].Equals(""))
                                {
                                    long.TryParse(datas[column], out longValue);
                                }

                                keyValuePairs.Add(keys[column], longValue);
                                break;
                            case "byte":
                                byte byteValue = 0;

                                if (!datas[column].Equals(""))
                                {
                                    byte.TryParse(datas[column], out byteValue);
                                }

                                keyValuePairs.Add(keys[column], byteValue);
                                break;
                            case "bool":
                                bool boolNum = false;

                                if (!datas[column].Equals(""))
                                {
                                    if (datas[column].Equals("TRUE"))
                                    {
                                        boolNum = true;
                                    }
                                    else if (datas[column].Equals("FALSE"))
                                    {
                                        boolNum = false;
                                    }
                                }

                                keyValuePairs.Add(keys[column], boolNum);
                                break;
                            case "Object":
                                break;
                            default:
                                break;
                        }
                    }

                    jArray.Add(keyValuePairs);
                }

                string path = "";
                path = Path.Combine(Application.dataPath + "/JsonFiles/", fileName + ".json");

                Debug.Log(fileName + " Create");
                File.WriteAllText(path, jArray.ToString());
            }

            EditorUtility.DisplayDialog("Success", "All Json successfully Save!", "OK");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}
#else

using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using System.IO;
public class SheetParsing : EditorWindow
{
    private static ListRequest _listRequest;
    private const string TargetPackageName = "com.unity.nuget.newtonsoft-json";
    private const string DefineSymbol = "JsonLoader";

    [InitializeOnLoadMethod]
    private static void CheckAndAddDefineSymbol()
    {
        _listRequest = Client.List();
        EditorApplication.update += Progress;
    }

    private static void Progress()
    {
        if (!_listRequest.IsCompleted) return;

        if (_listRequest.Status == StatusCode.Success)
        {
            bool packageExists = false;

            foreach (var package in _listRequest.Result)
            {
                if (package.name == TargetPackageName)
                {
                    packageExists = true;
                    break;
                }
            }

            if (!Directory.Exists("Assets/JsonFiles"))
            {
                Directory.CreateDirectory("Assets/JsonFiles");
            }

            if (packageExists)
            {
                AddDefineSymbol(DefineSymbol);
            }
            else
            {
                Debug.LogWarning($"Package '{TargetPackageName}' not found. Add Package '{TargetPackageName}'.");
            }
        }
        else if (_listRequest.Status >= StatusCode.Failure)
        {
            Debug.LogError($"Failed to list packages: {_listRequest.Error.message}");
        }

        EditorApplication.update -= Progress;
    }

    private static void AddDefineSymbol(string symbol)
    {
        var targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
        var currentSymbols = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);

        if (!currentSymbols.Contains(symbol))
        {
            currentSymbols = string.IsNullOrEmpty(currentSymbols)
                ? symbol
                : $"{currentSymbols};{symbol}";

            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, currentSymbols);
            Debug.Log($"Define symbol '{symbol}' added.");
        }
        else
        {
            Debug.Log($"Define symbol '{symbol}' already exists.");
        }
    }
}
#endif

#endif