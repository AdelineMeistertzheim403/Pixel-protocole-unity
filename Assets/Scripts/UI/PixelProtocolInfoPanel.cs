// Auto-generated from PixelProtocolInfoPanel.tsx
// Conversion React -> C# Unity UI
using UnityEngine;
using UnityEngine.UI;

public class PixelProtocolInfoPanel : MonoBehaviour
{
    public Text titleText;
    public Text infoText;

    public void SetTitle(string title)
    {
        if (titleText != null)
            titleText.text = title;
    }

    public void SetInfo(string info)
    {
        if (infoText != null)
            infoText.text = info;
    }
}
