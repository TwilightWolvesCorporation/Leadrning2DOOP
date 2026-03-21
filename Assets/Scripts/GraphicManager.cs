using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicManager : MonoBehaviour
{
    [SerializeField] private List<GraphicPreset> graphicPresets = new List<GraphicPreset>();

    private GraphicPreset _currentUrpGraphicPreset;
    private UniversalRenderPipelineAsset _currentUrpAsset;

    private void Awake()
    {
        GraphicManagerExtension.GraphicManager = this;
    }

    private void Start()
    {
        // SetGraphicPreset(GraphicManagerExtension.BinaryFileManager._data.qualityLevelIndex);
    }

    public void SetGraphicPreset(int graphicIndex)
    {
        _currentUrpGraphicPreset = graphicPresets.Find(preset => preset.QualityLevelIndex == graphicIndex);
        _currentUrpAsset = _currentUrpGraphicPreset.UrpAsset;
        ApplyGraphicPreset();
    }

    private void ApplyGraphicPreset()
    {
        QualitySettings.SetQualityLevel(_currentUrpGraphicPreset.QualityLevelIndex, true);

        GraphicsSettings.defaultRenderPipeline = _currentUrpAsset;

        // PlayerPrefs.SetInt("QualityLevelIndex", _currentUrpGraphicPreset.QualityLevelIndex);
        // PlayerPrefs.Save();

        Debug.Log("Cur Index" +_currentUrpGraphicPreset.QualityLevelIndex);
        
        GraphicManagerExtension.BinaryFileManager.SaveData(new BinaryData(_currentUrpGraphicPreset.QualityLevelIndex,
            _currentUrpGraphicPreset.QualityLevelIndex == 0 ? "Rus" : "Eng"));
    }
}

public static class GraphicManagerExtension
{
    public static GraphicManager GraphicManager;
    public static BinaryFileManager BinaryFileManager;
}