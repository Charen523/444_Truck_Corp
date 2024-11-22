using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private Dictionary<int, HeroData> Heros = new();
    private static Dictionary<string, List<object>> dataDics = new();

    protected override void Awake()
    {
        base.Awake();

        TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("Json");
        foreach (var jsonFile in jsonFiles)
        {
            string fileName = jsonFile.name;
            string jsonData = jsonFile.text;

            Type classType = Type.GetType(fileName);
            if (classType != null)
            {
                var jsonObject = JsonConvert.DeserializeObject(jsonData, typeof(List<>).MakeGenericType(classType));
                if (jsonObject is IEnumerable<object> objectList)
                {
                    foreach (var obj in objectList)
                    {
                        if (!dataDics.ContainsKey(fileName))
                        {
                            dataDics[fileName] = new List<object>();
                        }
                        dataDics[fileName].Add(obj);
                    }
                }
            }
            else
            {
                Debug.LogError($"'{fileName}'에 해당하는 클래스를 찾을 수 없습니다.");
            }
        }
    }

    public static object GetData(string className, int index)
    {
        return dataDics[className][index];
    }
}