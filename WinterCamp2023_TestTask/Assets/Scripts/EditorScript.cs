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

    private static int rocksCount = 50;

    [SerializeField]
    private static Vector3[] rocksPositions = new Vector3[]
    {
        new Vector3(-0.1f,0.04058f,0.172f), new Vector3(-0.075f,0.03965f,0.1776f), 
        new Vector3(-0.0475f,0.03785f,0.1757f), new Vector3(-0.03f,0.03544f,0.1532f), 
        new Vector3(-0.035f,0.03666f,0.1245f), new Vector3(-0.0581f,0.03886f,0.1083f), 
        new Vector3(-0.0812f,0.03906f,0.0921f), new Vector3(-0.0856f,0.03909f,0.0634f), 
        new Vector3(-0.0587f,0.03956f,0.0478f), new Vector3(-0.0331f,0.0423f,0.0565f), 
        new Vector3(-0.01f,0.04705f,0.069f), new Vector3(0.0131f,0.04108f,0.0846f), 
        new Vector3(0.0356f,0.03655f,0.0977f), new Vector3(0.0625f,0.03471f,0.1058f), 
        new Vector3(0.0906f,0.03401f,0.0996f), new Vector3(0.1106f,0.03385f,0.0784f), 
        new Vector3(0.1125f,0.03456f,0.0509f), new Vector3(0.1019f,0.03418f,0.0272f), 
        new Vector3(0.0725f,0.03454f,0.0197f), new Vector3(0.045f,0.03824f,0.0197f), 
        new Vector3(0.02f,0.04295f,0.0066f), new Vector3(0f,0.04247f,-0.0128f), 
        new Vector3(-0.0225f,0.0394f,-0.0284f), new Vector3(-0.0512f,0.03656f,-0.0284f), 
        new Vector3(-0.0799f,0.03457f,-0.0309f), new Vector3(-0.1068f,0.03466f,-0.0409f), 
        new Vector3(-0.1287f,0.03521f,-0.0559f), new Vector3(-0.1499f,0.03596f,-0.074f), 
        new Vector3(-0.1655f,0.03665f,-0.1046f), new Vector3(-0.1586f,0.03545f,-0.1327f), 
        new Vector3(-0.1368f,0.03602f,-0.152f), new Vector3(-0.1068f,0.03593f,-0.1539f), 
        new Vector3(-0.0806f,0.03573f,-0.1451f), new Vector3(-0.0581f,0.03452f,-0.127f), 
        new Vector3(-0.0387f,0.03505f,-0.1058f), new Vector3(-0.0168f,0.03709f,-0.0883f), 
        new Vector3(0.0101f,0.04139f,-0.0814f), new Vector3(0.0382f,0.04298f,-0.0727f), 
        new Vector3(0.0582f,0.03759f,-0.0508f), new Vector3(0.0807f,0.03452f,-0.0327f), 
        new Vector3(0.1044f,0.03375f,-0.0195f), new Vector3(0.1287f,0.03417f,-0.0076f), 
        new Vector3(0.1543f,0.03443f,0.0011f), new Vector3(0.1836f,0.03595f,-0.0008f), 
        new Vector3(0.2017f,0.03731f,-0.0264f), new Vector3(0.1942f,0.03783f,-0.0545f), 
        new Vector3(0.1723f,0.03668f,-0.0739f), new Vector3(0.1436f,0.03399f,-0.0764f), 
        new Vector3(0.1199f,0.03436f,-0.0864f), new Vector3(0.1105f,0.03758f,-0.1126f),
    };

    private Dictionary<int, string> rocksTypes = new Dictionary<int, string>()
    {
        { 0, "yellow" }, { 4, "pink"}, { 5, "blue" }, { 9, "pink" }, { 11, "red" }, 
        { 14, "pink" }, { 16, "blue" }, { 19, "pink" }, { 23, "red" }, { 24, "pink" }, 
        { 27, "blue" },  { 29, "pink" }, { 34, "pink" }, { 35, "red" }, { 38, "blue" }, 
        { 39, "pink" }, { 44, "pink" }, { 48, "red" }, { 49, "yellow" }
    };

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
        /*
        for (int i = 0; i < rocksCount; i++)
        {
            rocksPositions[i] = rockLineTransform.GetChild(i).position;
        }
        */

        var children = new List<GameObject>();
        foreach (Transform child in rockLineTransform) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
        int[] rockTypeKeys = rocksTypes.Keys.ToArray();

        for (int i = 0; i < rocksCount; i++)
        {
            Quaternion randomRockRotation = Quaternion.Euler(0, Random.Range(-180f, 180f), 0);
            if (rockTypeKeys.Contains(i))
            {
                Instantiate(rocksDict[rocksTypes[i]], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
            else
            {
                Instantiate(rocksDict["simple"], rocksPositions[i], randomRockRotation, rockLineTransform);
            }
        }
    }
}
