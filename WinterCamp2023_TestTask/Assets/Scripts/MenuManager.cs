using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private CameraController mainCamera;

    [SerializeField]
    private GameObject mainMenuBlock;

    [SerializeField]
    private GameObject modeMenuBlock;

    [SerializeField]
    private GameObject playerSettingsMenuBlock;
    [SerializeField]
    private TextMeshProUGUI playerText;

    [SerializeField]
    private Button[] colorButtons;

    [SerializeField]
    private GameObject gameUIBlock;
    [SerializeField]
    private TextMeshProUGUI turnText;
    [SerializeField]
    private TextMeshProUGUI gameInfoText;
    [SerializeField]
    private Button throwButton;

    [SerializeField]
    private GameObject gameResultsBlock;
    [SerializeField]
    private TextMeshProUGUI gameResultsText;


    private static string[] colorsTitles = new string[]
    {
        "blue", "red", "green", "yellow", "pink", "white"
    };

    private int playerCount = 0;
    private int selecterPlayer;

    private string[] playersData;

    private void Start()
    {
        gameManager.newTurnTextEvent.AddListener(SetTurnMessage);
        gameManager.uiMessageEvent.AddListener(SetInfoMessage);
        gameManager.gameOverEvent.AddListener(ShowGameResults);
    }

    public void NewGamePushed()
    {
        mainMenuBlock.SetActive(false);
        modeMenuBlock.SetActive(true);
    }

    public void BackToMainMenuPushed()
    {
        modeMenuBlock.SetActive(false);
        gameUIBlock.SetActive(false);
        gameResultsBlock.SetActive(false);
        mainMenuBlock.SetActive(true);
        GoToMenu();
        gameManager.QuitGame();
    }

    public void GameModePushed(int modeNumber)
    {
        playerCount = modeNumber;
        playersData = new string[playerCount];
        selecterPlayer = 0;
        modeMenuBlock.SetActive(false);
        playerText.text = $"Player {selecterPlayer + 1}";
        playerSettingsMenuBlock.SetActive(true);

        foreach (Button colorButton in colorButtons)
        {
            colorButton.interactable = true;
        }
    }

    public void ColorButtonPushed(int colorNumber)
    {
        colorButtons[colorNumber].interactable = false;
        playersData[selecterPlayer] = colorsTitles[colorNumber];
        selecterPlayer++;
        playerText.text = $"Player {selecterPlayer + 1}";
        if (selecterPlayer >= playerCount)
        {
            playerSettingsMenuBlock.SetActive(false);
            gameUIBlock.SetActive(true);
            turnText.text = "";
            gameInfoText.text = "";
            throwButton.interactable = false;
            StartGame();
        }
    }

    public void BackToModeMenuPushed()
    {
        playerSettingsMenuBlock.SetActive(false);
        modeMenuBlock.SetActive(true);
    }

    private void StartGame()
    {
        mainCamera.MoveToGameField();
        gameManager.StartGame(playerCount, playersData);
    }

    private void SetTurnMessage(int number)
    {
        turnText.text = $"Player {number + 1} turn";
        throwButton.interactable = true;
    }

    private void SetInfoMessage(string message)
    {
        gameInfoText.text = message;
    }

    private void ShowGameResults(string results)
    {
        gameUIBlock.SetActive(false);
        gameResultsText.text = results;
        gameResultsBlock.SetActive(true);
    }

    public void GoToMenu()
    {
        mainCamera.MoveToMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
