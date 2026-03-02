using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "GraphicLevelPreset", menuName = "Graphic Manager/GraphicLevelPreset")]
public class GraphicPreset : ScriptableObject
{
    public string PresetName;
    public int QualityLevelIndex;
    public UniversalRenderPipelineAsset UrpAsset;
}