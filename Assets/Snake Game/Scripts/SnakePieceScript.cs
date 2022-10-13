using System.Linq;
using UnityEngine;
using static Direction;

public class SnakePieceScript : MonoBehaviour
{
    // sprites
    public Sprite HeadRight;
    public Sprite HeadLeft;
    public Sprite HeadUp;
    public Sprite HeadDown;
    public Sprite BodyHorizontal;
    public Sprite BodyVertical;
    public Sprite TailRight;
    public Sprite TailLeft;
    public Sprite TailUp;
    public Sprite TailDown;
    public Sprite RotateRightToDown;
    public Sprite RotateRightToUp;
    public Sprite RotateLeftToUp;
    public Sprite RotateLeftToDown;

    public SnakeScript Snake { get; set; }
    public Direction Direction { get; set; }
    public Direction? PrevDirection { get; set; } // null if not rotated

    void Update()
    {
        if (Snake is null) return;

        var spriteRender = GetComponentInChildren<SpriteRenderer>();

        if (Snake.SnakePieces.FirstOrDefault() == this)
        {
            spriteRender.sprite = Direction switch
            {
                Right => HeadRight,
                Left => HeadLeft,
                Up => HeadUp,
                Down => HeadDown,
                _ => null
            };            
        }
        else
        if (Snake.SnakePieces.LastOrDefault() == this)
        {
            spriteRender.sprite = Direction switch
            {
                Right => TailLeft,
                Left => TailRight,
                Up => TailDown,
                Down => TailUp,
                _ => null
            };
        }
        else
        if (PrevDirection is null)
        {
            spriteRender.sprite = Direction switch
            {
                Right or Left => BodyHorizontal,
                Up or Down => BodyVertical,
                _ => null
            };
        }
        else
        {
            if ((PrevDirection is Right && Direction is Down)
                || (PrevDirection is Up && Direction is Left))
            {
                spriteRender.sprite = RotateRightToDown;
            }
            else
            if ((PrevDirection is Right && Direction is Up)
                || (PrevDirection is Down && Direction is Left))
            {
                spriteRender.sprite = RotateRightToUp;
            }
            else
            if ((PrevDirection is Left && Direction is Down)
                || (PrevDirection is Up && Direction is Right))
            {
                spriteRender.sprite = RotateLeftToDown;
            }
            else
            {
                spriteRender.sprite = RotateLeftToUp;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) 
        => Snake.OnCollision(collision.gameObject);
}