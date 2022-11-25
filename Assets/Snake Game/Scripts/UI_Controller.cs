using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    public GameController Game;
    public Button ContinueButton, RetryButton, SettingsButton;
    public TextMeshProUGUI LengthLabel, ScoresLabel, TimeLabel;
    public GameObject PausePanel, PropertiesPanel, LeaderboardPanel;
    public LeaderWriteView WriteNamePanel;

    void Start()
    {
        Game.ChangeGamemodeEvent += () =>
        {
            if (Game.Gamemode is GameMode.Play)
            {
                PropertiesPanel.SetActive(false);
                LeaderboardPanel.SetActive(false);
                WriteNamePanel.gameObject.SetActive(false);
                PausePanel.SetActive(false);
            }
            else if (Game.Gamemode is GameMode.Pause)
            {
                SettingsButton.interactable = false;
                ContinueButton.interactable = true;

                PropertiesPanel.SetActive(false);
                LeaderboardPanel.SetActive(false);
                WriteNamePanel.gameObject.SetActive(false);
                PausePanel.SetActive(true);
            }
            else if (Game.Gamemode is GameMode.GameOver)
            {
                SettingsButton.interactable = true;
                ContinueButton.interactable = false;

                PropertiesPanel.SetActive(false);
                LeaderboardPanel.SetActive(false);
                if (Game.Scores == 0)
                {
                    WriteNamePanel.gameObject.SetActive(false);
                    PausePanel.SetActive(true);
                }
                else
                {
                    WriteNamePanel.gameObject.SetActive(true);
                    PausePanel.SetActive(false);
                }
            }
        };
    }

    private void Update()
    {
        if (Game.Gamemode is GameMode.Play)
        {
            LengthLabel.text = $"Length: {Game.GetSnakeLength}";
            ScoresLabel.text = $"Scores: {Game.Scores}";
            TimeLabel.text = TimeSpan.FromSeconds(Game.EllapsedTime).ToString(@"hh\:mm\:ss");
        }
    }

    public void ShowProperties()
    {
        PropertiesPanel.SetActive(true);
    }
    public void HideProperties()
    {
        PropertiesPanel.SetActive(false);
    }
    public void ShowLeaderboard()
    {
        LeaderboardPanel.SetActive(true);
    }
    public void HideLeaderboard()
    {
        LeaderboardPanel.SetActive(false);
    }
    public void HideWriteNamePanel()
    {
        WriteNamePanel.SaveToLeaderboard();
        WriteNamePanel.gameObject.SetActive(false);
        PausePanel.SetActive(true);
    }
}