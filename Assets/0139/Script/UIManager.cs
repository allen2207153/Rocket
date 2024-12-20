using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();

    public void RegisterUI(string name, GameObject uiObject)
    {
        if (!uiElements.ContainsKey(name))
        {
            uiElements[name] = uiObject;
        }
    }

    public void ShowUI(string name)
    {
        if (uiElements.ContainsKey(name))
        {
            uiElements[name].SetActive(true);
        }
    }

    public void HideUI(string name)
    {
        if (uiElements.ContainsKey(name))
        {
            uiElements[name].SetActive(false);
        }
    }

    public void ToggleUI(string name)
    {
        if (uiElements.ContainsKey(name))
        {
            bool isActive = uiElements[name].activeSelf;
            uiElements[name].SetActive(!isActive);
        }
    }

    public void HideAllUI(List<string> exclude = null)
    {
        foreach (var kvp in uiElements)
        {
            if (exclude == null || !exclude.Contains(kvp.Key))
            {
                kvp.Value.SetActive(false);
            }
        }
    }
}
