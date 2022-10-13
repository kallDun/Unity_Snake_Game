using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnFruitsScript : MonoBehaviour
{
    public GameObject gameField;
    public SnakeScript snake;

    public AudioSource audioSource;
    public AudioClip eat_sound;

    public GameObject fruit;    
    public int max_count;

    public List<GameObject> Fruits { get; } = new();

    public void SpawnFruits(bool recursed = false)
    {
        for (int i = Fruits.Count; i < max_count + (recursed ? 1 : 0); i++)
        {
            var spawned = Instantiate(fruit, transform);

            spawned.transform.position = GetRandomEmptyPosition();
            var script = spawned.GetComponent<FruitScript>();

            int type = new System.Random().Next(0, 3);
            script.fruitType = (FruitType)type;            

            float size = gameField.GetComponent<TileFieldScript>().TileSize;
            spawned.transform.localScale = new Vector3(size, size, 1);

            Fruits.Add(spawned);
            script.OnEating += () =>
            {
                GameObject fr = spawned;
                FruitScript sc = script;
                SpawnFruits(true);
                Fruits.Remove(fr);
                script.Hide();
                Destroy(fr, 1.5f);
                audioSource.PlayOneShot(eat_sound);
            };
        }
    }
    private Vector3 GetRandomEmptyPosition()
    {
        var field = gameField.GetComponent<TileFieldScript>().Field;
        var snakePieces = snake.SnakePieces;

        List<Vector3> empties = new();
        for (int y = 0; y < field.Length; y++)
        {
            for (int x = 0; x < field[y].Length; x++)
            {
                if (!snakePieces.Any(item => 
                    item.gameObject.transform.position == field[y][x].transform.position)
                    || !Fruits.Any(item =>
                    item.gameObject.transform.position == field[y][x].transform.position))
                {
                    empties.Add(field[y][x].transform.position);
                }
            }
        }
        int rand_index = new System.Random().Next(0, empties.Count);
        return empties[rand_index];
    }
    public void DestroyFruits()
    {
        foreach (var item in Fruits)
        {
            Destroy(item);
        }
        Fruits.Clear();
    }    
}