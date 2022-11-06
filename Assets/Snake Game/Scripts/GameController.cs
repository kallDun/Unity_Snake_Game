using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    // scores
    public int Scores { get; private set; }
    public void AddScores(int value)
    {
        if (value > 0) Scores += value;
    }

    // gamemode
    public GameMode Gamemode { get; private set; }
    public event Action ChangeGamemodeEvent;

    // timer
    public float EllapsedTime { get; private set; }

    // length
    public int GetSnakeLength => _snake.SnakePieces.Count;

    // -----------------------------------
    private SnakeController _snake;
    private TileFieldSpawner _tileField;
    private FruitsSpawner _spawnFruits;

    void Start()
    {
        _snake = GetComponentInChildren<SnakeController>();
        _tileField = GetComponentInChildren<TileFieldSpawner>();
        _spawnFruits = GetComponentInChildren<FruitsSpawner>();
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
        _tileField.DestroyField();
        _tileField.GenerateField();
        _snake.DestroySnake();
        _snake.SpawnSnake();
        _spawnFruits.DestroyFruits();
        _spawnFruits.SpawnFruits();

        EllapsedTime = 0;
        Scores = 0;
        Gamemode = GameMode.Play;
        ChangeGamemodeEvent?.Invoke();
    }
    public void GameOver()
    {
        Gamemode = GameMode.GameOver;
        ChangeGamemodeEvent?.Invoke();
    }
    public void Pause()
    {
        if (Gamemode == GameMode.Play)
        {
            Gamemode = GameMode.Pause;
            ChangeGamemodeEvent?.Invoke();
        }        
    }
    public void Continue()
    {
        if (Gamemode == GameMode.Pause)
        {
            Gamemode = GameMode.Play;
            ChangeGamemodeEvent?.Invoke();
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
        _tileField.TileCount_Height = height;
        _tileField.TileCount_Width = width;
        _tileField.TileSize = size;
        _snake.BaseSpeed = speed;
    }
    public void ChangeRandomField(Toggle toggle)
    {
        _tileField.IsProcedureRandom = toggle.isOn;
    }
    public void ChangeWallTeleprotation(Toggle toggle)
    {
        _snake.IsTeleportatingBetweenWalls = toggle.isOn;
    }
    public void ChangeFruitsCount(TMP_Dropdown dropdown)
    {
        _spawnFruits.max_count = dropdown.value + 1;
    }
}