using TMPro;
using UnityEngine;

public class UILocalText : MonoBehaviour
{
    [SerializeField] string textKey;

    private void Awake()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        text.text = DataManager.Instance.GetText(textKey);
    }
}
