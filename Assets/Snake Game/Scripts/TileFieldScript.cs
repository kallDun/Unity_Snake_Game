using System.Collections.Generic;
using UnityEngine;

public class TileFieldScript : MonoBehaviour
{
    public GameObject Center;
    public GameObject Tile, Wall;
    public Color WallColor;
    public Color[] TileColorVariations;
    public float TileSize = 1f;
    public int TileCount_Width = 10;
    public int TileCount_Height = 10;
    public bool IsProcedureRandom = true;

    public GameObject[][] Field { get; private set; }
    private List<GameObject> Walls { get; } = new();

    void Start() => Center.SetActive(false);

    public void GenerateField()
    {
        Field = new GameObject[TileCount_Height][];

        float GetPosY(int index_y) => Center.transform.position.y - (TileSize * (TileCount_Height / 2)) + (index_y * TileSize);
        float GetPosX(int index_x) => Center.transform.position.x - (TileSize * (TileCount_Width / 2)) + (index_x * TileSize);

        int tileColor = 0;
        for (int y = 0; y < TileCount_Height; y++)
        {
            Field[y] = new GameObject[TileCount_Width];
            for (int x = 0; x < TileCount_Width; x++)
            {
                GameObject tile = Instantiate(Tile, gameObject.transform);
                tile.transform.localScale = new Vector3(TileSize, TileSize, 1);
                tile.transform.position = new Vector3(GetPosX(x), GetPosY(y), 1);

                SpriteRenderer sprite = tile.GetComponentInChildren<SpriteRenderer>();
                sprite.color = TileColorVariations[tileColor];

                tileColor += IsProcedureRandom ? new System.Random().Next(0, 10) : 1;
                tileColor %= TileColorVariations.Length;

                Field[y][x] = tile;
            }
            tileColor++;
            tileColor %= TileColorVariations.Length;
        }

        // spawn walls
        for (int y = 0; y < TileCount_Height; y++)
        {
            SpawnWall(Field[y][0].transform.position + new Vector3(-TileSize, 0, 0), angle: 0);
            SpawnWall(Field[y][TileCount_Width - 1].transform.position + new Vector3(TileSize, 0, 0), angle: 180);
        }
        for (int x = 0; x < TileCount_Width; x++)
        {
            SpawnWall(Field[0][x].transform.position + new Vector3(0, -TileSize, 0),  angle: 90);
            SpawnWall(Field[TileCount_Height - 1][x].transform.position + new Vector3(0, TileSize, 0), angle: 270);
        }
    }
    private void SpawnWall(Vector3 position, int angle)
    {
        GameObject wall = Instantiate(Wall, gameObject.transform);
        wall.transform.localScale = new Vector3(TileSize, TileSize, 1);
        wall.transform.position = position;
        wall.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        SpriteRenderer sprite = wall.GetComponentInChildren<SpriteRenderer>();
        sprite.color = WallColor;
        Walls.Add(wall);
    }
    public void DestroyField()
    {
        if (Field is null) return;
        for (int i = 0; i < Field.Length; i++)
        {
            if (Field[i] is null) continue;
            for (int j = 0; j < Field[i].Length; j++)
            {
                Destroy(Field[i][j]);
            }
        }
        foreach (var item in Walls)
        {
            Destroy(item);
        }
        Walls.Clear();
    }
}