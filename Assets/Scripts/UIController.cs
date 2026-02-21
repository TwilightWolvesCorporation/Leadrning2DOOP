using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InputActionAsset inputPlayer;
    [SerializeField] private InputActionReference inputPause;
    
    private bool _isPaused = false;

    [SerializeField] private PlayerInput input;
    private string _savePath;
    [SerializeField] private List<SelectKey>  selectKeys;
    

    private void Awake()
    {
        _savePath = Path.Combine(Application.dataPath, "InputBindings.json");
        LoadBindings();
        inputPause.action.performed += SetActionPause;
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

    private void SetActionPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        SetPause();
    }
    public void SetHp(Slider slider)
    {
        hpText.text = slider.value.ToString();
        playerController.SetHp((int)slider.value);
    }

    public void SetPause()
    {
        _isPaused = !_isPaused;
        pauseCanvas.gameObject.SetActive(_isPaused);

        if (_isPaused) inputPlayer.actionMaps[0].Disable();
        else inputPlayer.actionMaps[0].Enable();
    }

    public void Restart()
    {
        SaveBindings();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Exit()
    {
        SaveBindings();
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        inputPause.action.performed -= SetActionPause;
    }
}