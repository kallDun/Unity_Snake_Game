using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScript : MonoBehaviour
{
    // scores
    public int Scores { get; private set; }
    public void AddScores(int value)
    {
        if (value > 0) Scores += value;
    }

    // gamemode
    public GameMode Gamemode { get; private set; }
    public event Action OnChangingGamemode;

    // timer
    public float EllapsedTime { get; private set; }

    // length
    public int GetSnakeLength => snake.SnakePieces.Count;

    // -----------------------------------
    SnakeScript snake;
    TileFieldScript tileField;
    SpawnFruitsScript spawnFruits;

    void Start()
    {
        snake = GetComponentInChildren<SnakeScript>();
        tileField = GetComponentInChildren<TileFieldScript>();
        spawnFruits = GetComponentInChildren<SpawnFruitsScript>();
        GameOver();
    }
    private void Update()
    {
        if (Gamemode == GameMode.Play)
        {
            EllapsedTime += Time.deltaTime;
        }        
    }

    public void RestartGame()
    {
        tileField.DestroyField();
        tileField.GenerateField();
        snake.DestroySnake();
        snake.SpawnSnake();
        spawnFruits.DestroyFruits();
        spawnFruits.SpawnFruits();

        EllapsedTime = 0;
        Scores = 0;
        Gamemode = GameMode.Play;
        OnChangingGamemode?.Invoke();
    }
    public void GameOver()
    {
        Gamemode = GameMode.GameOver;
        OnChangingGamemode?.Invoke();
    }
    public void Pause()
    {
        if (Gamemode == GameMode.Play)
        {
            Gamemode = GameMode.Pause;
            OnChangingGamemode?.Invoke();
        }        
    }
    public void Continue()
    {
        if (Gamemode == GameMode.Pause)
        {
            Gamemode = GameMode.Play;
            OnChangingGamemode?.Invoke();
        }
    }

    // -----------------------------------
    public void ChangeFieldSize(TMP_Dropdown dropdown)
    {
        (int height, int width, float size, float speed) = dropdown.value switch
        {
            0 => (30, 23, .2f, 10f),
            1 => (20, 15, .25f, 8f),
            2 => (45, 33, .13f, 14f),
            3 or _ => (60, 45, .1f, 16f)
        };
        tileField.TileCount_Height = height;
        tileField.TileCount_Width = width;
        tileField.TileSize = size;
        snake.base_speed = speed;
    }
    public void ChangeRandomField(Toggle toggle)
    {
        tileField.IsProcedureRandom = toggle.isOn;
    }
    public void ChangeFruitsCount(TMP_Dropdown dropdown)
    {
        spawnFruits.max_count = dropdown.value + 1;
    }
}