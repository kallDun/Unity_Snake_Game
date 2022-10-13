using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour
{
    public GameScript Game;
    public Button ContinueButton, RetryButton, SettingsButton;
    public TextMeshProUGUI LengthLabel, ScoresLabel, TimeLabel;
    public GameObject PausePanel, PropertiesPanel;

    void Start()
    {
        Game.OnChangingGamemode += () =>
        {
            if (Game.Gamemode is GameMode.Play)
            {
                PropertiesPanel.SetActive(false);
                PausePanel.SetActive(false);
            }
            if (Game.Gamemode is GameMode.Pause)
            {
                SettingsButton.interactable = false;
                ContinueButton.interactable = true;
                PropertiesPanel.SetActive(false);
                PausePanel.SetActive(true);
            }
            if (Game.Gamemode is GameMode.GameOver)
            {
                SettingsButton.interactable = true;
                ContinueButton.interactable = false;
                PropertiesPanel.SetActive(false);
                PausePanel.SetActive(true);
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
}