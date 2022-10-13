using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    public GameScript game;
    public GameObject snakePiece;
    public TileFieldScript tileField;
    public int startLength = 4;
    public float base_speed = 10f;
    float speed;
    Direction direction;

    public List<SnakePieceScript> SnakePieces { get; } = new List<SnakePieceScript>();

    public void SpawnSnake()
    {
        int center_y = tileField.Field.Length / 2;
        int center_x = tileField.Field[0].Length / 2;

        for (int i = 0; i < startLength; i++)
        {
            GameObject snakeObj = Instantiate(snakePiece, transform);
            snakeObj.transform.localScale = new Vector3(tileField.TileSize, tileField.TileSize, 1);
            snakeObj.transform.localPosition = tileField.Field[center_y][center_x - i].transform.localPosition;

            SnakePieceScript snakeScript = snakeObj.GetComponent<SnakePieceScript>();
            snakeScript.Direction = Direction.Right;
            snakeScript.Snake = this;
            SnakePieces.Add(snakeScript);
        }

        direction = Direction.Right;
        speed = base_speed;
    }
    public void DestroySnake()
    {
        foreach (var item in SnakePieces)
        {
            Destroy(item.gameObject);
        }
        SnakePieces.Clear();
    }

    // --------------------------
    public void Move(Direction direction)
    {
        //raycast

        // rotation if needed
        var head = SnakePieces.First();
        if (head.Direction != direction)
        {
            head.PrevDirection = head.Direction;
            head.Direction = direction;
        }

        // if is growing
        if (grow_on > 0)
        {
            grow_on--;
            GameObject snakeObj = Instantiate(snakePiece, transform);
            snakeObj.transform.localScale = new Vector3(tileField.TileSize, tileField.TileSize, 1);
            var script = snakeObj.GetComponent<SnakePieceScript>();
            script.Snake = this;
            SnakePieces.Add(script);
        }

        // offset every part to next by list
        for (int i = SnakePieces.Count - 1; i > 0; i--)
        {
            SnakePieces[i].Direction = SnakePieces[i - 1].Direction;
            SnakePieces[i].PrevDirection = SnakePieces[i - 1].PrevDirection;
            SnakePieces[i].gameObject.transform.position =
                SnakePieces[i - 1].gameObject.transform.position;
        }

        // translate head position
        static Vector3 GetVectorFromDirection(Direction dir) => dir switch
        {
            Direction.Right => new(1, 0, 0),
            Direction.Left => new(-1, 0, 0),
            Direction.Up => new(0, 1, 0),
            Direction.Down => new(0, -1, 0),
            _ => new(0, 0, 0)
        };
        head.gameObject.transform.position += GetVectorFromDirection(direction) * tileField.TileSize;

        // head and tail not rotate
        head.PrevDirection = null;
        SnakePieces.Last().PrevDirection = null;
    }

    float time;    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (game.Gamemode == GameMode.Play)
        {
            time += Time.deltaTime;
            float val = 1f / speed;
            if (time >= val)
            {
                time -= val;
                Move(direction);
            }
        }
    }
    public void MoveLeft()
    {
        if (direction is not Direction.Right) direction = Direction.Left;
    }
    public void MoveRight()
    {
        if (direction is not Direction.Left) direction = Direction.Right;
    }
    public void MoveDown()
    {
        if (direction is not Direction.Up) direction = Direction.Down;
    }
    public void MoveUp()
    {
        if (direction is not Direction.Down) direction = Direction.Up;
    }

    // --------------------------
    int grow_on = 0;
    public void OnCollision(GameObject gameObject)
    {
        if (game.Gamemode is not GameMode.Play) return;

        var fruit = gameObject.GetComponent<FruitScript>();
        if (fruit is not null)
        {
            grow_on++;
            switch (fruit.fruitType)
            {
                case FruitType.GrowUp:
                    grow_on++;
                    game.AddScores(2);
                    break;
                case FruitType.SpeedUp:
                    speed += base_speed * 0.3f;
                    if (speed > base_speed * 2f) speed = base_speed * 2f;
                    game.AddScores(3);
                    break;
                case FruitType.SlowDown:
                    speed -= base_speed * 0.3f;
                    if (speed < base_speed * 0.25f) speed = base_speed * 0.25f;
                    game.AddScores(1);
                    break;
            }
            return;
        }
        // if its not a fruit (wall, snake) - game over
        game.GameOver();
    }
}