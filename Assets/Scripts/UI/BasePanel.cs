using UnityEngine;

public class BasePanel : MonoBehaviour
{
    [SerializeField] GameObject back;
    [SerializeField] bool isTranslucent;

    public bool IsTranslucent => isTranslucent;

    public virtual bool IsEscafePossible { get {return true;} }

    public virtual void FirstLoad()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void Open()
    {
        if (back != null)
        {
            back.SetActive(true);
        }
    }

    public virtual void Close()
    {
        if (back != null)
        {
            back.SetActive(false);
        }
    }

    public virtual void CallEscafe()
    {
        UIManager.Instance.PopPanel();
    }
}
