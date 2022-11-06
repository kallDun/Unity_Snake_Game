using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public GameController Game;
    public GameObject SnakePiece;
    public TileFieldSpawner TileField;
    public int StartLength = 4;
    public float BaseSpeed = 10f;
    public bool IsTeleportatingBetweenWalls = false;

    public List<SnakePiece> SnakePieces { get; } = new List<SnakePiece>();

    private float _speed;
    private Direction _prevDirection, _currDirection;
    private float _time;
    private int _growOn = 0;

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

        if (Game.Gamemode == GameMode.Play)
        {
            _time += Time.deltaTime;
            float val = 1f / _speed;
            if (_time >= val)
            {
                _time -= val;
                _prevDirection = _currDirection;
                Move(_currDirection);
            }
        }
    }

    public void SpawnSnake()
    {
        int center_y = TileField.Field.Length / 2;
        int center_x = TileField.Field[0].Length / 2;

        for (int i = 0; i < StartLength; i++)
        {
            GameObject snakeObj = Instantiate(SnakePiece, transform);
            snakeObj.transform.localScale = new Vector3(TileField.TileSize, TileField.TileSize, 1);
            snakeObj.transform.localPosition = TileField.Field[center_y][center_x - i].transform.localPosition;

            SnakePiece snakeScript = snakeObj.GetComponent<SnakePiece>();
            snakeScript.Direction = Direction.Right;
            snakeScript.Snake = this;
            SnakePieces.Add(snakeScript);
        }

        _prevDirection = Direction.Right;
        _currDirection = Direction.Right;
        _speed = BaseSpeed;
    }
    public void DestroySnake()
    {
        foreach (var item in SnakePieces)
        {
            Destroy(item.gameObject);
        }
        SnakePieces.Clear();
    }

    public void Move(Direction direction)
    {
        Vector3? teleportationPosition = null;
        var head = SnakePieces.First();

        // check raycast on wall or snake
        var raycastDirection = GetVectorFromDirection(direction);
        var raycastOrigin = head.transform.position + raycastDirection * (TileField.TileSize / 2);
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, raycastDirection, TileField.TileSize * 0.5f);
        Debug.DrawLine(raycastOrigin, raycastOrigin + TileField.TileSize * raycastDirection, Color.red, 1f / _speed);
        if (hit.transform is not null)
        {
            print("Collide!");
            GameObject hittedObj = hit.transform.gameObject;
            var wall = hittedObj.GetComponent<Wall>();
            if (wall is not null)
            {
                if (IsTeleportatingBetweenWalls)
                {
                    teleportationPosition = TileField.GetTeleportationLocationFromWall(wall);
                }
                else
                {
                    Game.GameOver();
                    return;
                }
            }
            else
            {
                var snake = hittedObj.GetComponent<SnakePiece>();
                if (snake is not null)
                {
                    Game.GameOver();
                    return;
                }
            }
        }

        // rotation if needed
        if (head.Direction != direction)
        {
            head.PrevDirection = head.Direction;
            head.Direction = direction;
        }

        // if is growing
        if (_growOn > 0)
        {
            _growOn--;
            GameObject snakeObj = Instantiate(SnakePiece, transform);
            snakeObj.transform.localScale = new Vector3(TileField.TileSize, TileField.TileSize, 1);
            var script = snakeObj.GetComponent<SnakePiece>();
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
        if (teleportationPosition is not null)
        {
            head.gameObject.transform.position = (Vector3)teleportationPosition;
        }
        else
        {
            head.gameObject.transform.position += GetVectorFromDirection(direction) * TileField.TileSize;
        }

        // head and tail not rotate
        head.PrevDirection = null;
        SnakePieces.Last().PrevDirection = null;
    }
    public void MoveLeft()
    {
        if (_prevDirection is not Direction.Right)
        {
            _currDirection = Direction.Left;
        }
    }
    public void MoveRight()
    {
        if (_prevDirection is not Direction.Left)
        {
            _currDirection = Direction.Right;
        }
    }
    public void MoveDown()
    {
        if (_prevDirection is not Direction.Up)
        {
            _currDirection = Direction.Down;
        }
    }
    public void MoveUp()
    {
        if (_prevDirection is not Direction.Down)
        {
            _currDirection = Direction.Up;
        }
    }

    public void OnCollision(GameObject gameObject)
    {
        if (Game.Gamemode is not GameMode.Play) return;

        var fruit = gameObject.GetComponent<FruitController>();
        if (fruit is not null)
        {
            _growOn++;
            switch (fruit.fruitType)
            {
                case FruitType.GrowUp:
                    _growOn++;
                    Game.AddScores(2);
                    break;
                case FruitType.SpeedUp:
                    _speed += BaseSpeed * 0.3f;
                    if (_speed > BaseSpeed * 2f) _speed = BaseSpeed * 2f;
                    Game.AddScores(3);
                    break;
                case FruitType.SlowDown:
                    _speed -= BaseSpeed * 0.3f;
                    if (_speed < BaseSpeed * 0.25f) _speed = BaseSpeed * 0.25f;
                    Game.AddScores(1);
                    break;
            }
            return;
        }
        /*// if its not a fruit (wall, snake) - game over
        game.GameOver();*/
    }
}