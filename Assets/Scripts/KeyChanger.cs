using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyChanger : MonoBehaviour
{
    private SelectKey _selectKey;
    private bool _isChanging = false;

    private void Update()
    {
        if (!_isChanging) return;
        if(Keyboard.current == null) return;
        foreach (var key in Keyboard.current.allKeys.Where(key => key.wasPressedThisFrame))
        {
            SetKey(key.keyCode.ToString().ToUpper());
            return;
        }
    }

    public void SelectKey(SelectKey selectKey)
    {
        _selectKey = selectKey;
        _isChanging = true;
    }

    private void SetKey(string key)
    {
        _selectKey.GetAction().action.Disable();
        _selectKey.GetAction().action.ApplyBindingOverride(0, $"<Keyboard>/{key}");
        _selectKey.SetText();
        gameObject.SetActive(false);
    }
}