using ClientEnum;
using TMPro;
using UnityEngine;

public partial class UIManager //UI
{
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text lifeText;
    [SerializeField] TMP_Text coinText;

    public void SetText(UIType uIType,string value)
    {
        switch (uIType)
        {
            case UIType.Wave:
                waveText.text = value;
                break;
            case UIType.Life:
                lifeText.text = value;
                break;
            case UIType.Coin:
                coinText.text = value;
                break;
            default:
                break;
        }
    }
}
