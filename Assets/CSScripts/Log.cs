using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    [SerializeField] private Toggle _checkBox;
    [SerializeField] private Image _selectionImage;
    [SerializeField] private string _questDescription;

    public int ProgressStep;
    public Toggle TextToggle;

    public void ToggleCheckBoxOn()
    {
        _checkBox.isOn = true;
    }
    
    public void ToggleCheckBoxOff()
    {
        _checkBox.isOn = false;
    }
    public void DisableImage()
    {
        _selectionImage.gameObject.SetActive(false);
    }
    
    public void EnableImage()
    {
        _selectionImage.gameObject.SetActive(true);
    }

    public string GetDescription()
    {
        return _questDescription;
    }

    public void ChangeDescriptionText(string text)
    {
        _questDescription = text;
    }
    
    
}