using System;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    public Sprite fruit_1, fruit_2, fruit_3;
    public ParticleSystem particleSys;
    public SpriteRenderer spriteRender;
    public FruitType fruitType { get; set; }

    public event Action OnEating;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SnakePiece>() is not null)
        {
            OnEating?.Invoke();            
        }        
    }
    private void Update()
    {
        spriteRender.sprite = fruitType switch
        {
            FruitType.SpeedUp => fruit_1,
            FruitType.SlowDown => fruit_2,
            FruitType.GrowUp or _ => fruit_3
        };
    }

    public void Hide()
    {
        spriteRender.enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        particleSys.textureSheetAnimation.SetSprite(0, spriteRender.sprite);
        particleSys.Play();
    }
}