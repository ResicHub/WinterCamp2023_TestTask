using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BezierGenerator
{
    private LineRenderer rocksLineRenderer;

    private int smoothIndex;
    private float smoothStep;

    private Dictionary<Vector3, List<Vector3>> controlPoints =
        new Dictionary<Vector3, List<Vector3>>();

    private int anchorsCount;

    private List<Vector3> anchors = new List<Vector3>();

    public void CreateCurve(Transform rocksMarks, LineRenderer lineRenderer, int smoothIndexValue)
    {
        smoothIndex = smoothIndexValue;
        rocksLineRenderer = lineRenderer;
        smoothStep = 1 / (float)smoothIndex;

        anchorsCount = rocksMarks.childCount;
        foreach (Transform anchorTransform in rocksMarks)
        {
            anchors.Add(anchorTransform.position);
        }

        SetControlPoints();
        SmoothOut();
    }

    /// <summary>
    /// Sets up control points for each anchor.
    /// </summary>
    private void SetControlPoints()
    {
        controlPoints.Clear();
        // Setting up control points for each ahcnor of curve except extreme ahcnors (start and end).
        for (int index = 0; index < anchorsCount - 2; index++)
        {
            SetControlPointsForAnchor(anchors[index], anchors[index + 1], anchors[index + 2]);
        }

        // Setting up control points for extreme ahcnors.
        SetControlPointsForAnchor(anchors[0], anchors[0], anchors[1]);
        SetControlPointsForAnchor(anchors[anchorsCount - 2], anchors[anchorsCount - 1], anchors[anchorsCount - 1]);
    }

    private void SetControlPointsForAnchor(Vector3 anchor1, Vector3 anchor2, Vector3 anchor3)
    {
        Vector3 tangent = (anchor3 - anchor1) / 3f;
        controlPoints.Add(anchor2, new List<Vector3>()
        {
            anchor2 - tangent,
            anchor2 + tangent
        });
    }

    /// <summary>
    /// Creates segments of curve with Bezier algorithm.
    /// </summary>
    private void SmoothOut()
    {
        rocksLineRenderer.positionCount = 1;
        rocksLineRenderer.SetPosition(0, anchors[0]);

        int index = 1;
        // Creating of segments between two next curve anchors.
        for (int i = 0; i < anchorsCount - 1; i++)
        {
            CreateCurveSevment(anchors[i], anchors[i + 1], ref index);
        }
    }

    /// <summary>
    /// Creates a Bezier curve segment between two ahcnors.
    /// </summary>
    private void CreateCurveSevment(Vector3 anchor1, Vector3 anchor2, ref int index)
    {
        rocksLineRenderer.positionCount += smoothIndex;
        for (float part = smoothStep; part < 1 + smoothStep; part += smoothStep)
        {
            rocksLineRenderer.SetPosition(index,
                CubicLerp(
                    anchor1,
                    controlPoints[anchor1][1],
                    controlPoints[anchor2][0],
                    anchor2,
                    part));
            index++;
        }
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0 = Vector3.Lerp(a, b, t);
        Vector3 p1 = Vector3.Lerp(b, c, t);
        return Vector3.Lerp(p0, p1, t);
    }

    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 p0 = QuadraticLerp(a, b, c, t);
        Vector3 p1 = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(p0, p1, t);
    }
}