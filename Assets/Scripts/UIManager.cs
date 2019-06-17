using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Button[] buttons;
    public Color selectedColor;
    private void Awake()
    {
        UpdateButtons((int)ModesManager.mode);
    }
    public void UpdateMode(int i)
    {
        ModesManager.mode=(OperationMode)i;
        UpdateButtons(i);
    }
    public void ResetScrew() { }
    void UpdateButtons(int activeIndex)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock colorBlock= buttons[i].colors;
            colorBlock.normalColor = i==activeIndex ? selectedColor: Color.white;
            colorBlock.highlightedColor = colorBlock.normalColor;
            buttons[i].colors = colorBlock;
        }        
    }
}
