using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectKey : MonoBehaviour
{
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private InputActionReference inputAction;

    private void Awake()
    {
        SetText();
    }

    private void Start()
    {
        SetText();
    }

    public void SetText()
    {
        var path = inputAction.action.bindings[0].effectivePath;
        var keys = path.Split('/');
        keyText.text = keys[^1].ToUpper();
    }

    public InputActionReference GetAction()
    {
        return inputAction; 
    }
    
}