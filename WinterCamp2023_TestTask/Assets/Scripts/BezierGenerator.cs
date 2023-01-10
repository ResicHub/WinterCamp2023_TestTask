using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class BezierGenerator : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private Vector3 curveHeight = new Vector3(0f, 0.003f, 0f);
    private int smoothIndex = 10;

    private Dictionary<Vector3, List<Vector3>> controlPoints =
        new Dictionary<Vector3, List<Vector3>>();

    private Vector3[] anchors = new Vector3[2];

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void CreateMovementTrajectory(Vector3 startPosition, Vector3 targetPosition)
    {
        anchors[0] = startPosition;
        anchors[1] = targetPosition;

        SetControlPoints();
        SmoothOut();
    }

    /// <summary>
    /// Sets up control points for each anchor.
    /// </summary>
    private void SetControlPoints()
    {
        controlPoints.Clear();
        // Setting up control points for extreme ahcnors.
        SetControlPointsForAnchor(anchors[0], anchors[0], anchors[0] + curveHeight);
        SetControlPointsForAnchor(anchors[1] + curveHeight, anchors[1], anchors[1]);
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
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, anchors[0]);

        int index = 1;
        // Creating of segments between two next curve anchors.
        CreateCurveSevment(anchors[0], anchors[1], ref index);
    }

    /// <summary>
    /// Creates a Bezier curve segment between two ahcnors.
    /// </summary>
    private void CreateCurveSevment(Vector3 anchor1, Vector3 anchor2, ref int index)
    {
        float smoothStep = 1 / (float)smoothIndex;
        lineRenderer.positionCount += smoothIndex;
        for (float part = smoothStep; part < 1 + smoothStep; part += smoothStep)
        {
            lineRenderer.SetPosition(index,
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