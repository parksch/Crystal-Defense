using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IHasWeight
{
    public float GetWeight();
}
public interface IDrawable
{
    public object Draw();
}

public enum Grade
{
    Normal,
    Rare,
    SuperRare,
    UltraRare
}

public interface IHasGrade
{
    public Grade GetGrade();
}

public class DrawBox : IHasWeight, IDrawable, IHasGrade, System.IComparable
{
    public object data;
    public float weight;
    public Grade grade;

    public DrawBox(object data, float weight)
    {
        this.data = data;
        this.weight = weight;
    }

    public int CompareTo(object obj)
    {
        if (obj.GetType() != typeof(DrawBox)) return -1;
        if (obj == null) return -1;

        DrawBox data = obj as DrawBox;
        if (data.data != this.data) return -1;
        if (data.weight != this.weight) return -1;

        return 0;
    }

    public object Draw()
    {
        return data;
    }

    public Grade GetGrade()
    {
        return grade;
    }

    public float GetWeight()
    {
        return weight;
    }
}

public class DrawHandler
{
    private Dictionary<int, float /* max */> capturedMax = new Dictionary<int, float>();
    private const int DontCaptured = -1;

    //== 단챠
    public T SingleDraw<T>(int key, List<T> deck) where T : IHasWeight, IDrawable, System.IComparable
    {
        if (!capturedMax.TryGetValue(key, out float max))
        {
            max = 0;
            deck.ForEach((v) => max += v.GetWeight());

            if (key != DontCaptured)
            {
                capturedMax[key] = max;
            }
        }

        float choice = Random.Range(0, max);

        T result = default(T);
        for (int i = 0; i < deck.Count; i++)
        {
            choice -= deck[i].GetWeight();

            if (choice <= 0)
            {
                result = deck[i];
                break;
            }
        }

        if (result.CompareTo(default(T)) == 0) throw new System.Exception("Table Error : Draw Table");

        return (T)result.Draw();
    }

    //== 연속 뽑기
    public List<T> MultiDraw<T>(int key, int count, List<T> deck) where T : IHasWeight, IDrawable, System.IComparable
    {
        List<T> result = new List<T>();

        for (int i = 0; i < count; i++)
        {
            result.Add(SingleDraw<T>(key, deck));
        }

        return result;
    }

    //== 최소 1개는 최소 등급 이상 등장
    public List<T> AtLeastOneGradeDraw<T>(int key, int count, Grade minGrade, List<T> deck) where T : IHasWeight, IDrawable, IHasGrade, System.IComparable
    {
        List<T> result = (1 < count) ? MultiDraw<T>(key, count - 1, deck) : new List<T>();
        List<T> filtered = ListPool<T>.Get();

        for (int i = 0; i < deck.Count; i++)
        {
            var box = deck[i];
            if (minGrade <= box.GetGrade())
            {
                filtered.Add(box);
            }
        }

        result.Add(SingleDraw(DontCaptured, filtered));
        ListPool<T>.Release(filtered);

        return result;
    }
}

public class Run
{
    Dictionary<int, (int /* DrawLimit  */,Grade)> drawLimit = new Dictionary<int, (int, Grade )>();
    DrawHandler drawHandler = new DrawHandler();
    const int centerDraw = 10;
    const Grade centerGrade = Grade.SuperRare;

    public void DrawStart(int tableID,int count)
    {
        if (drawLimit.TryGetValue(tableID,out (int count,Grade grade) limit))
        {
            //== 테이블 로드를 하여 저장
        }

        int targetCount = count;
        int currentCount = 100;//데이터 매니져에서 가져왔다고 가정

        List<DrawBox> drawBoxes = MakingDrawList(tableID);
        List<DrawBox> resultList = new List<DrawBox>();

        if (currentCount + targetCount >= limit.count)
        {
            targetCount--;
            resultList.AddRange(drawHandler.AtLeastOneGradeDraw(tableID, 1, limit.grade, drawBoxes));
        }

        if (count == centerDraw)
        {
            targetCount--;
            resultList.AddRange(drawHandler.AtLeastOneGradeDraw(tableID, 1, centerGrade, drawBoxes));
        }

        resultList.AddRange(drawHandler.MultiDraw(tableID, targetCount, drawBoxes));

        foreach (var item in resultList)
        {
            DataProcessing(item);
        }

        // 데이터 매니져 횟수 처리 
    }

    public List<DrawBox> MakingDrawList(int tableID)
    {
        //== 여기서 혼합해서 넣고 하면
        //== 저긴 진짜 뽑기만 담당하게 된다
        return null;
    }

    public void DataProcessing(DrawBox box)
    {

    }
}