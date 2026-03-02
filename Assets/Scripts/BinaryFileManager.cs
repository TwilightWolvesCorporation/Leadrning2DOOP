using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryFileManager : MonoBehaviour
{
    private string _savePath;
    public BinaryData _data;

    private void Awake()
    {
        GraphicManagerExtension.BinaryFileManager = this;
        _savePath = Application.persistentDataPath + "/saveFile.dat";
    }

    private void Start()
    {
        LoadData();
    }

    public void SaveData(BinaryData data)
    {
        if(data == null) return;
        _data = data;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_savePath, FileMode.Create);
        formatter.Serialize(stream, _data);
        stream.Close();
    }

    private void LoadData()
    {
        try
        {
            var formatter = new BinaryFormatter();
            var stream = new FileStream(_savePath, FileMode.Open);
            _data = (BinaryData)formatter.Deserialize(stream);
            stream.Close();
        }
        catch (Exception e)
        {
            SaveData(new BinaryData(1, "ex"));
        }


        ApplyData();
    }

    private void ApplyData()
    {
        if (_data == null) return;
        GraphicManagerExtension.GraphicManager.SetGraphicPreset(_data.qualityLevelIndex);
        Debug.Log(_data.qualityLevelIndex);
        Debug.Log(_data.language);
    }
}

[Serializable]
public class BinaryData
{
    public int qualityLevelIndex;
    public string language;

    public BinaryData(int _qualityLevelIndex, string _language)
    {
        qualityLevelIndex = _qualityLevelIndex;
        language = _language;
    }
}