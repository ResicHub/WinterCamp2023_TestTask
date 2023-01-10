using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLineGenerator : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    private float pathLength = 0f;
    private float step = 0f;

    public List<Vector3> RocksLineCoordinates(LineRenderer rocksLineRenderer, int rocksCount)
    {
        lineRenderer = rocksLineRenderer;

        
        pathLength = CalculatePathLength();
        step = pathLength / rocksCount;

        List<Vector3> coordinates = new List<Vector3>();
        coordinates.Add(lineRenderer.GetPosition(0));

        float routeOnLine = 0f;
        for (int pointIndex = 0; pointIndex < lineRenderer.positionCount - 1; pointIndex++)
        {
            Vector3 vectorToNextPoint = lineRenderer.GetPosition(pointIndex + 1)
                - lineRenderer.GetPosition(pointIndex);

            routeOnLine += vectorToNextPoint.magnitude;
            if (routeOnLine > step)
            {
                routeOnLine -= step;

                coordinates.Add(lineRenderer.GetPosition(pointIndex + 1) 
                    - vectorToNextPoint);
            }
        }
        coordinates.Add(lineRenderer.GetPosition(lineRenderer.positionCount - 1));
        return coordinates;
    }

    private float CalculatePathLength()
    {
        float pathLen = 0f;
        // Calculate the sum of each curve segment.
        for (int pointIndex = 0; pointIndex < lineRenderer.positionCount - 1; pointIndex++)
        {
            pathLen += (lineRenderer.GetPosition(pointIndex + 1) 
                - lineRenderer.GetPosition(pointIndex))
                .magnitude; 
        }
        return pathLen;
    }
}
