using TMPro;
using UnityEngine;

public class UIValueText : MonoBehaviour
{
    [SerializeField] TMP_Text left;
    [SerializeField] TMP_Text right;

    public void ResetText()
    {
        left.text = "";
        right.text = "";
    }

    public void SetText(string leftText,string rightText)
    {
        left.text += leftText + "\n";
        right.text += rightText + "\n";
    }
}
