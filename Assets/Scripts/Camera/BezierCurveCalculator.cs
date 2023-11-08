using System;
using UnityEngine;

public class BezierCurveCalculator: MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /**
     * Calculates a cubic Bezier curve for the camera to move along.
     * @param start: The point the bezier curve will start at
     * @param end: The point the bezier curve will end at
     * @param linearity: A float value between 0 and 1 
     */
    public static Vector3 CalculateCurve(Vector3 start, Vector3 end, float linearity = 1f / 16f)
    {
        Vector3 startForward = GetDirection(start, end);
        Vector3 endForward = GetDirection(end, start);

        return Mathf.Pow(1f - linearity, 3f) * start + 3f *
               Mathf.Pow(1f - linearity, 2f) * linearity * startForward + 3f * (1f - linearity) *
               Mathf.Pow(linearity, 2f) * endForward +
               Mathf.Pow(linearity, 3f) * end;
    }

    /**
     * Calculate direction between two points
     */
    private static Vector3 GetDirection(Vector3 start, Vector3 end)
    {
        return Vector3.zero;
    }
}
