using System;
using System.Globalization;
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

    private void Awake()
    {
        inputPause.action.performed += SetActionPause;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnDestroy()
    {
        inputPause.action.performed -= SetActionPause;
    }
}