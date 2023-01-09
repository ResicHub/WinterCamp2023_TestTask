using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EditorScript : MonoBehaviour
{
    private static Dictionary<string, GameObject> rocksDict = new Dictionary<string, GameObject>()
    {
        { "simple", null },
        { "red", null },
        { "blue", null },
        { "yellow", null },
        { "pink", null }
    };

    [SerializeField]
    private GameObject[] stonesObjects = new GameObject[rocksDict.Count];

    [SerializeField]
    private Transform rocksTransform;

    [SerializeField]
    private Transform rockLineTransform;

    [SerializeField]
    private RockLineGenerator rockLineGenerator;

    private BezierGenerator bezierGenerator = new BezierGenerator();

    [SerializeField]
    [Range(1, 10)]
    private int smoothIndex = 1;

    [SerializeField]
    private int rocksCount { get; set; }

    private void Awake()
    {
        string[] keys = rocksDict.Keys.ToArray();
        for (int i = 0; i < rocksDict.Count; i++)
        {
            rocksDict[keys[i]] = stonesObjects[i];
        }
    }

    public void GenerateRockLine()
    {
        foreach (Transform child in rockLineTransform)
        {
            Destroy(child.gameObject);
        }

        LineRenderer rocksLineRenderer = rocksTransform.GetComponent<LineRenderer>();
        bezierGenerator.CreateCurve(rocksTransform, rocksLineRenderer, smoothIndex);
        List<Vector3> coordinatesOfRocks = rockLineGenerator.RocksLineCoordinates(rocksLineRenderer, rocksCount);

        foreach (Vector3 position in coordinatesOfRocks)
        {
            Instantiate(rocksDict["yellow"], position, Quaternion.identity, rockLineTransform);
        }
    }
}
