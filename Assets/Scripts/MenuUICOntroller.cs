using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    private string _savePath;
    [SerializeField] private List<SelectKey>  selectKeys;
    

    private void Awake()
    {
        _savePath = Path.Combine(Application.dataPath, "InputBindings.json");
        LoadBindings();
    }

    private void LoadBindings()
    {
        if (!File.Exists(_savePath)) return;
        var jsonBindings = File.ReadAllText(_savePath);
        input.actions.LoadBindingOverridesFromJson(jsonBindings);
    }

    private void SaveBindings()
    {
        var jsonBindings = input.actions.SaveBindingOverridesAsJson();
        File.WriteAllText(_savePath, jsonBindings);
    }

    public void ResetToDefaultBindings()
    {
        input.actions.RemoveAllBindingOverrides();
        if (File.Exists(_savePath)) File.Delete(_savePath);
        foreach (var selectKey in selectKeys) selectKey.SetText();
    }

    public void SetFullscreenMode(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void StartGame()
    {
        SaveBindings();
        SceneManager.LoadScene($"Level_1");
    }

    public void Exit()
    {
        SaveBindings();
        Application.Quit();
    }
}