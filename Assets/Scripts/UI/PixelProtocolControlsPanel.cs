// Auto-generated from PixelProtocolControlsPanel.tsx
// Conversion React -> C# Unity UI
using UnityEngine;
using UnityEngine.UI;

public class PixelProtocolControlsPanel : MonoBehaviour
{
    public Text infoText;
    public Button jumpButton;
    public Button dashButton;
    public Button hackButton;
    // Ajoute ici les autres boutons ou éléments UI nécessaires

    void Start()
    {
        // Exemple d’abonnement à un bouton
        if (jumpButton != null)
            jumpButton.onClick.AddListener(OnJumpClicked);
        // ... autres abonnements
    }

    public void SetInfo(string message)
    {
        if (infoText != null)
            infoText.text = message;
    }

    void OnJumpClicked()
    {
        // Logique à exécuter lors du clic sur le bouton Sauter
        Debug.Log("Jump button clicked");
    }

    // Ajoute ici les autres méthodes pour gérer les interactions UI
}
