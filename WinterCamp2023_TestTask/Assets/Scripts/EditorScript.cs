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
    private Transform rockLineTransform;

    private int rocksCount;

    private void Awake()
    {
        string[] keys = rocksDict.Keys.ToArray();
        for (int i = 0; i < rocksDict.Count; i++)
        {
            rocksDict[keys[i]] = stonesObjects[i];
        }
    }

    /*
    [SerializeField]
    private Transform rocksMarksTransform;

    [SerializeField]
    private RockLineGenerator rockLineGenerator;

    private BezierGenerator bezierGenerator = new BezierGenerator();

    [SerializeField]
    [Range(1, 10)]
    private int smoothIndex = 1;

    public void GenerateRockLine()
    {
        int destroyCounter = 0;
        foreach (Transform child in rockLineTransform)
        {
            DestroyImmediate(child.gameObject);
            destroyCounter++;
        }
        Debug.Log($"Destroying {destroyCounter} number of objects");

        LineRenderer rocksLineRenderer = rocksMarksTransform.GetComponent<LineRenderer>();
        bezierGenerator.CreateCurve(rocksMarksTransform, rocksLineRenderer, smoothIndex);
        List<Vector3> coordinatesOfRocks = rockLineGenerator.RocksLineCoordinates(rocksLineRenderer, rocksCount);

        foreach (Vector3 position in coordinatesOfRocks)
        {
            Instantiate(rocksDict["yellow"], position, Quaternion.identity, rockLineTransform);
        }
    }
    */

    public void ReCreateRocks()
    {
        rocksCount = rockLineTransform.childCount;
        Vector3[] rocksPositions = new Vector3[rocksCount];

        for (int i = 0; i < rocksCount; i++)
        {
            rocksPositions[i] = rockLineTransform.GetChild(i).position;
        }

        var children = new List<GameObject>();
        foreach (Transform child in rockLineTransform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));

        for (int i = 0; i < rocksCount; i++)
        {
            Quaternion randomRockRotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);

            if (i == 0 || i == rocksCount - 1)
            {
                Instantiate(rocksDict["yellow"], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
            else if (i % 5 == 0)
            {
                Instantiate(rocksDict["pink"], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
            else if (i % 11 == 6)
            {
                Instantiate(rocksDict["blue"], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
            else if (i % 12 == 0)
            {
                Instantiate(rocksDict["red"], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
            else
            {
                Instantiate(rocksDict["simple"], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
        }
    }
}
