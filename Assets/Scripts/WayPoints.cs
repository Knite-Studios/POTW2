using UnityEngine;

public class WayPoints : MonoBehaviour
{
    public static Transform[] points;

    private void Awake()
    {
        points = new Transform[transform.childCount];
        for (var i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}