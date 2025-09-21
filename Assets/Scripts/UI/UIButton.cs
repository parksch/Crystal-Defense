using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UIButton : MonoBehaviour
{
    [SerializeField] Button targetButton;
    [SerializeField] Image targetImage;
    [SerializeField] Sprite ableSprite;
    [SerializeField] Sprite disableSprite;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color ableTextColor;
    [SerializeField] Color disableTextColor;

    private void Awake()
    {
        targetButton = GetComponent<Button>();
        targetImage = GetComponent<Image>();
    }

    public void SetInteractive(bool onOff)
    {
        if (targetButton == null)
        {
            targetButton = GetComponent<Button>();
        }

        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        targetButton.interactable = onOff;

        if (onOff)
        {
            targetImage.sprite = ableSprite;
            
            if (text != null)
            {
                text.color = ableTextColor;
            }
        }
        else
        {
            targetImage.sprite = disableSprite;

            if (text != null)
            {
                text.color = disableTextColor;
            }
        }
    }
}
