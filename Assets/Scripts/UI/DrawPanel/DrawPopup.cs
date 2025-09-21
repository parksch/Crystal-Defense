using ClientEnum;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DrawPopup : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text drawHierarchyText;
    [SerializeField] List<UICard> cardList;
    [SerializeField] UILastCard lastCard;
    [SerializeField] GameObject drawObject;
    [SerializeField] GameObject selectObject;

    [SerializeField] string drawTitleKey;

    DrawHierarchy drawResult;

    public void Init()
    {
        lastCard.SetCallBack(() =>
        {
            drawHierarchyText.gameObject.SetActive(true);
            drawHierarchyText.transform.DOPunchScale(new Vector3(.4f, .4f, .4f), .3f, 10, 1f).OnComplete(() =>
            {
                selectObject.SetActive(true);
            });
        });
    }

    public void Set()
    {
        titleText.text = DataManager.Instance.GetText(drawTitleKey);

        ResetCard();
        CardRandDraw();

        gameObject.SetActive(true);
    }

    public void OnClickRedraw()
    {
        ResetCard();
        CardRandDraw();
    }

    public DrawHierarchy OpenCard()
    {
        drawObject.SetActive(false);

        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].OpenCard();
        }

        return drawResult;
    }

    void ResetCard()
    {
        drawHierarchyText.gameObject.SetActive(false);
        drawObject.SetActive(true);
        selectObject.SetActive(false);

        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].Rewind();
        }
    }

    void CardRandDraw()
    {
        drawResult = DrawHierarchy.High;
        List<Draw> randCard = new List<Draw>();

        for (int i = 0; i < cardList.Count; i++)
        {
            Draw randDraw = (Draw)Random.Range((int)Draw.One, (int)Draw.Max);

            randCard.Add(randDraw);
        }

        randCard = randCard.OrderBy(x => (int)x).ToList();
        drawResult = CheckRand(randCard);

        SetResultText(drawResult);

        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].SetImage(randCard[i]);
        }
    }

    DrawHierarchy CheckRand(List<Draw> randCard)
    {
        for (int i = (int)Draw.One; i < (int)Draw.Max; i++)
        {
            int count = 0;

            for (int j = 0; j < randCard.Count; j++)
            {
                if (randCard[j] == (Draw)i)
                {
                    count++;
                }
            }

            if (count == 2)
            {
                for (int k = (int)Draw.One; k < (int)Draw.Max; k++)
                {
                    if (k == i)
                    {
                        continue;
                    }

                    int count2 = 0;

                    for (int l = 0; l < randCard.Count; l++)
                    {
                        if (randCard[l] == (Draw)k)
                        {
                            count2++;
                        }
                    }

                    if (count == 2 && count2 == 2)
                    {
                        return DrawHierarchy.DoublePair;
                    }
                }
            }

            if (count == 4)
            {
                return DrawHierarchy.Four;
            }
            if (count == 3)
            {
                return DrawHierarchy.Triple;
            }
            if (count == 2)
            {
                return DrawHierarchy.Pair;
            }
        }

        for (int i = 0; i < randCard.Count; i++)
        {
            if (randCard[i] != (Draw)i + 1)
            {
                break;
            }
            else if (i == randCard.Count - 1 && randCard[i] == (Draw)randCard.Count)
            {
                return DrawHierarchy.Straight;
            }
        }

        return DrawHierarchy.High;
    }

    void SetResultText(DrawHierarchy draw)
    {
        switch (draw)
        {
            case DrawHierarchy.High:
                drawHierarchyText.text = "High";
                break;
            case DrawHierarchy.Pair:
                drawHierarchyText.text = "Pair";
                break;
            case DrawHierarchy.DoublePair:
                drawHierarchyText.text = "DoublePair";
                break;
            case DrawHierarchy.Triple:
                drawHierarchyText.text = "Triple";
                break;
            case DrawHierarchy.Straight:
                drawHierarchyText.text = "Straight";
                break;
            case DrawHierarchy.Four:
                drawHierarchyText.text = "Four";
                break;
            default:
                break;
        }
    }
}
