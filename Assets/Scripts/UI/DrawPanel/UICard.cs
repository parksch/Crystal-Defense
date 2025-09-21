using ClientEnum;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.ComponentModel;

public class UICard : MonoBehaviour
{
    public Draw DrawValue => randomValue;

    [SerializeField] int index;
    [SerializeField] GameObject frontObject;
    [SerializeField] List<DotPos> dotPosList;
    [SerializeField] List<GameObject> dots = new List<GameObject>();
    [SerializeField] Draw randomValue;

    protected Sequence cardSequqence;

    [System.Serializable]
    class DotPos
    {
        [SerializeField] Draw target;
        [SerializeField] List<Transform> transforms = new List<Transform>();

        public Draw Target => target;
        public List<Transform> Transforms => transforms;
    }

    public void Rewind()
    {
        if (cardSequqence != null)
        {
            cardSequqence.Rewind();
        }
    }

    public void SetImage(Draw draw)
    {
        randomValue = draw;
        VisualCard();
    }

    public virtual void OpenCard()
    {
        if (cardSequqence == null)
        {
            cardSequqence = DOTween.Sequence().SetAutoKill(false);
            cardSequqence.OnStart(() => { frontObject.transform.localPosition = Vector3.zero;
                                          frontObject.GetComponent<Image>().color = Color.white;});
            cardSequqence.Append(frontObject.transform.DOLocalMoveY(-300,1f).SetEase(Ease.InSine));
            cardSequqence.Join(frontObject.GetComponent<Image>().DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.InExpo));
            cardSequqence.SetDelay(index * 0.2f);
        }
        else
        {
            cardSequqence.Restart();
        }
    }

    void VisualCard()
    {
        DotPos dotpos = dotPosList.Find(x => x.Target == randomValue);

        for (int i = 0; i < dots.Count; i++)
        {
            dots[i].SetActive(false);
        }

        for (int i = 0; i < dotpos.Transforms.Count ; i++)
        {
            dots[i].SetActive(true);
            dots[i].transform.position = dotpos.Transforms[i].position;
        }
    }

}
