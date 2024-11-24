using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, List<object>> DataDics { get; private set; } = new(); //Resources/Json으로부터 자동 로딩

    [SerializeField] private List<Sprite> thumbnails; //StartScene에서 수동캐싱
    [SerializeField] private List<Sprite> standUIs; //StartScene에서 수동캐싱
    [SerializeField] private List<string> characterSheetPaths; //StartScene에서 수동캐싱

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
                        if (!DataDics.ContainsKey(fileName))
                        {
                            DataDics[fileName] = new List<object>();
                        }
                        DataDics[fileName].Add(obj);
                    }
                }
            }
            else
            {
                Debug.LogError($"'{fileName}'에 해당하는 클래스를 찾을 수 없습니다.");
            }
        }
    }

    public T GetData<T>(string className, int index)
    {
        return (T)DataDics[className][index];
    }

    public List<T> GetDataList<T> (string key) where T : class
    {
        if (!DataDics.TryGetValue(key, out var objectList))
        {
            Debug.LogWarning($"{key} 클래스 존재하지 않음.");
            return new List<T>();
        }

        var dataList = new List<T>();
        foreach (var obj in objectList)
        {
            if (obj is T data) dataList.Add(data);
            else Debug.LogWarning($"객체를 {typeof(T)}로 변환 불가.");
        }

        return dataList;
    }

    public Sprite GetSprites(bool isStand, int idx)
    {
        if (isStand) return standUIs[idx];
        else return thumbnails[idx];
    }

    public string GetCharacterSheetPath(int idx)
    {
        return characterSheetPaths[idx];
    }
}