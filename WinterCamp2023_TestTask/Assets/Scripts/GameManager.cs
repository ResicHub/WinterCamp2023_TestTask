using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GameManager : MonoBehaviour
{
    private BezierGenerator bezierGenerator;
    private LineRenderer lineRenderer;

    private Transform mainCamera;
    private float mainCameraSpeed = 50f;

    private Vector3[] stepsPositions = new Vector3[]
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

    private int[] rockTypesKeys;

    private static Dictionary<string, GameObject> gamePiecesDict = new Dictionary<string, GameObject>()
    {
        { "while", null },
        { "red", null },
        { "blue", null },
        { "yellow", null },
        { "pink", null },
        { "green", null }
    };

    [SerializeField]
    private GameObject[] gamePiecesObjects = new GameObject[gamePiecesDict.Count];

    [SerializeField]
    private float gamePieceMovementSpeed = 0.1f;

    private int playersCount = 6;

    private Player[] players;
    private int activePlayerNumber = -1;
    private List<int> inactivePlayers = new List<int>();

    private Transform[] playersGamePieces;

    private bool isPlayerCanThrow;

    private void Awake()
    {
        bezierGenerator = GetComponent<BezierGenerator>();
        lineRenderer = GetComponent<LineRenderer>();

        isPlayerCanThrow = false;
        mainCamera = Camera.main.transform;
        rockTypesKeys = rocksTypes.Keys.ToArray();

        string[] keys = gamePiecesDict.Keys.ToArray();
        for (int i = 0; i < gamePiecesDict.Count; i++)
        {
            gamePiecesDict[keys[i]] = gamePiecesObjects[i];
        }
    }

    private void Start()
    {
        players = new Player[playersCount];
        playersGamePieces = new Transform[playersCount];

        players[0] = new Player("Tom", "red");
        players[1] = new Player("Greg", "blue");
        players[2] = new Player("Tom", "yellow");
        players[3] = new Player("Greg", "green");
        players[4] = new Player("Tom", "pink");
        players[5] = new Player("Greg", "while");

        StartCoroutine(StartGameCoroutine());
    }

    private void FixedUpdate()
    {
        if (isPlayerCanThrow)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPlayerCanThrow = false;
                int steps = Random.Range(1, 7);
                Debug.LogWarning($"Number {steps} on the cube");
                Turn(steps);
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            mainCamera.RotateAround(Vector3.zero, Vector3.up, -mainCameraSpeed * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            mainCamera.RotateAround(Vector3.zero, Vector3.up, mainCameraSpeed * Time.fixedDeltaTime);
        }
    }

    private IEnumerator StartGameCoroutine()
    {
        float playerPositionOffset = 0.006f;
        for (int playerNumber = 0; playerNumber < playersCount; playerNumber++)
        {
            Transform gamePiece =  Instantiate(gamePiecesDict[players[playerNumber].Color],
                stepsPositions[0], 
                Quaternion.Euler(0, 60 * playerNumber, 0), 
                transform).transform;
            gamePiece.position += gamePiece.forward * playerPositionOffset;
            playersGamePieces[playerNumber] = gamePiece;
        }
        SetActivePlayer(0);
        yield return null;
    }

    private void Turn(int steps)
    {
        Player player = players[activePlayerNumber];
        Transform gamePiece = playersGamePieces[activePlayerNumber];
        int nextStep = player.Step + steps;
        if (nextStep >= stepsPositions.Length)
        {
            nextStep = stepsPositions.Length - 1;
        }
        else if (nextStep < 0)
        {
            nextStep = 0;
        }

        Vector3 nextStepPosition = gamePiece.position + (stepsPositions[nextStep] - stepsPositions[player.Step]);
        player.Step = nextStep;

        StartCoroutine(GamePieceMovingCoroutine(gamePiece, nextStepPosition));
    }

    private IEnumerator GamePieceMovingCoroutine(Transform gamePiece, Vector3 targetPosition)
    {
        bezierGenerator.CreateMovementTrajectory(gamePiece.position, targetPosition);
        yield return new WaitForSeconds(1f);
        Vector3 nextPoint;
        Vector3 direction;
        int nextPointIndex = 1;

        while (nextPointIndex < lineRenderer.positionCount)
        {
            nextPoint = lineRenderer.GetPosition(nextPointIndex);
            direction = (nextPoint - gamePiece.position).normalized;

            yield return gamePiece.position += direction * gamePieceMovementSpeed * Time.fixedDeltaTime;

            if ((nextPoint - gamePiece.position).magnitude <= 0.001)
            {
                nextPointIndex++;
            }
        }
        gamePiece.position = targetPosition;
        yield return new WaitForSeconds(1f);
        CheckStepType(players[activePlayerNumber].Step);
    }

    private void CheckStepType(int step)
    {
        if (rockTypesKeys.Contains(step))
        {
            if (rocksTypes[step] == "red")
            {
                Debug.Log("Red rock: go back to 3 steps");
                Turn(- 3);
            }
            else if (rocksTypes[step] == "blue")
            {
                Debug.Log("Blue rock: go forward to 3 steps");
                Turn(3);
            }
            else if (rocksTypes[step] == "pink")
            {
                Debug.Log("Pink rock: WOW, that was really cool!");
                SetActivePlayer((activePlayerNumber + 1) % playersCount);
            }
            else if (rocksTypes[step] == "yellow")
            {
                if (step == 0)
                {
                    Debug.Log("Lol, you are on start again!");
                }
                else
                {
                    Debug.Log($"Player {activePlayerNumber + 1} WON!");
                    inactivePlayers.Add(activePlayerNumber);
                }
                if (inactivePlayers.Count < playersCount)
                {
                    SetActivePlayer((activePlayerNumber + 1) % playersCount);
                }
                else
                {
                    Debug.Log("GAME OVER!");
                }
            }
        }
        else
        {
            Debug.Log("Simple - dimple");
            SetActivePlayer((activePlayerNumber + 1) % playersCount);
        }
    }

    private void SetActivePlayer(int playerNumber)
    {
        if (inactivePlayers.Contains(playerNumber))
        {
            SetActivePlayer((playerNumber + 1) % playersCount);
        }
        else
        {
            activePlayerNumber = playerNumber;
            isPlayerCanThrow = true;
            Debug.Log($"Player {playerNumber + 1} turn");
        }
    }
}

public class Player
{
    private string _name;
    public string Name
    {
        get
        {
            return _name;
        }
    }

    private string _color;
    public string Color
    {
        get
        {
            return _color;
        }
    }

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
        }
    }

    private int _step;
    public int Step 
    {
        get
        {
            return _step;
        }
        set
        {
            _step = value;
        }
    }

    /// <summary>
    /// Creates a new player with a few parameters.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="color"></param>
    public Player(string name, string color)
    {
        _name = name;
        _color = color;
        _score = 0;
        _step = 0;
    }
}
