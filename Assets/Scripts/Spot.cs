using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [SerializeField] ClientEnum.Arrow arrow;
    [SerializeField] bool isStart;
    [SerializeField] bool isEnd;

    public bool IsEnd => isEnd;

    public Vector3 GetDiraction
    {
        get
        {
            switch (arrow)
            {
                case ClientEnum.Arrow.Stop:
                    return Vector3.zero;
                case ClientEnum.Arrow.Left:
                    return Vector3.left;
                case ClientEnum.Arrow.Right:
                    return Vector3.right;
                case ClientEnum.Arrow.Up:
                    return Vector3.forward;
                case ClientEnum.Arrow.Down:
                    return Vector3.back;
                default:
                    break;
            }

            return Vector3.zero;
        }
    }
}
