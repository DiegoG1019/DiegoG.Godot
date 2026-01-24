using Godot;

namespace DiegoG.Godot.Common;

public struct DirectionMovementState(int x, int y)
{
    public int InitialX { get; set; } = x;
    public int InitialY { get; set; } = y;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public Vector2I Position
    {
        get => new(X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }
    public CardinalDirection CardinalDirection { get; set; }

    public void Move()
    {
        switch (CardinalDirection)
        {
            case CardinalDirection.West:
                X++;
                break;
            case CardinalDirection.East:
                X--;
                break;
            case CardinalDirection.South:
                Y++;
                break;
            case CardinalDirection.North:
                Y--;
                break;
            case CardinalDirection.NorthWest:
                Y--;
                X++;
                break;
            case CardinalDirection.SouthWest:
                Y++;
                X++;
                break;
            case CardinalDirection.SouthEast:
                Y++;
                X--;
                break;
            case CardinalDirection.NorthEast:
                Y--;
                X--;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void MoveWithinBounds(Rect2 rectangle, BoundsCheckReaction boundsCheck = BoundsCheckReaction.Stop)
        => MoveWithinBounds(new((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height), boundsCheck);

    public bool TryMoveWithinBounds(Rect2 rectangle)
        => TryMoveWithinBounds(new((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height));

    public Vector2I TruncatePositionWithinBounds(Rect2 rectangle)
        => TruncatePositionWithinBounds(new((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height));

    public Vector2I TruncatePositionWithinBounds(Rect2I rectangle)
        => new(int.Clamp(X, 0, rectangle.Width), int.Clamp(Y, 0, rectangle.Height));
    
    public bool TryMoveWithinBounds(Rect2I rectangle)
    {
        var prev = Position;
        Move();

        if ((CardinalDirection is CardinalDirection.East && X >= rectangle.Left)
            || (CardinalDirection is CardinalDirection.West && X < rectangle.Right)
            || (CardinalDirection is CardinalDirection.North && Y >= rectangle.Top)
            || (CardinalDirection is CardinalDirection.South && Y < rectangle.Bottom))
        {
            CheckPosition(rectangle);
            return true;
        }

        Position = prev;
        CheckPosition(rectangle);
        return false;
    }
    
    public void MoveWithinBounds(Rect2I rectangle, BoundsCheckReaction boundsCheck = BoundsCheckReaction.Stop)
    {
        var prev = Position;
        Move();
        if (CheckPosition(rectangle))
            return;

        if (boundsCheck is BoundsCheckReaction.Stop)
        {
            if (CardinalDirection is CardinalDirection.East && X >= rectangle.Left
                || CardinalDirection is CardinalDirection.West && X < rectangle.Right
                || CardinalDirection is CardinalDirection.North && Y >= rectangle.Top
                || CardinalDirection is CardinalDirection.South && Y < rectangle.Bottom)
                return;
            
            Position = prev;
        }

        if (CardinalDirection is CardinalDirection.East or CardinalDirection.West && X == rectangle.Left || X + 1 >= rectangle.Right)
        {
            switch (boundsCheck)
            {
                case BoundsCheckReaction.Bounce:
                    Position = prev;
                    CardinalDirection = CardinalDirection switch
                    {
                        CardinalDirection.West => CardinalDirection.East,
                        CardinalDirection.East => CardinalDirection.West,
                        _ => throw new InvalidOperationException()
                    };
                    Move();
                    break;
                case BoundsCheckReaction.Slide:
                    Position = prev;
                    CardinalDirection = Y - rectangle.Top > rectangle.Bottom - Y ? CardinalDirection.North : CardinalDirection.South;
                    Move();
                    break;
                case BoundsCheckReaction.Reset or BoundsCheckReaction.ResetAndChangeDirection:
                {
                    X = InitialX;
                    Y = InitialY;
                    if (boundsCheck is BoundsCheckReaction.ResetAndChangeDirection) 
                        RandomizeDirection();
                    Move();
                    break;
                }
            }
        }
        else if (CardinalDirection is CardinalDirection.North && Y <= rectangle.Top || CardinalDirection is CardinalDirection.South && Y + 1 >= rectangle.Bottom)
        {
            switch (boundsCheck)
            {
                case BoundsCheckReaction.Bounce:
                    Position = prev;
                    CardinalDirection = CardinalDirection switch
                    {
                        CardinalDirection.North => CardinalDirection.South,
                        CardinalDirection.South => CardinalDirection.North,
                        _ => throw new InvalidOperationException()
                    };
                    Move();
                    break;
                case BoundsCheckReaction.Slide:
                    Position = prev;
                    CardinalDirection = X - rectangle.Left > rectangle.Right - X ? CardinalDirection.East : CardinalDirection.West;
                    Move();
                    break;
                case BoundsCheckReaction.Reset or BoundsCheckReaction.ResetAndChangeDirection:
                {
                    X = InitialX;
                    Y = InitialY;
                    if (boundsCheck is BoundsCheckReaction.ResetAndChangeDirection) 
                        RandomizeDirection();
                    Move();
                    break;
                }
            }
        }

#if DEBUG
        CheckPosition(rectangle);
#endif
        //TODO: This looks ugly as hell. Revisit, please
    }

    public void RandomizeDirection(Random? random = null)
    {
        CardinalDirection = (CardinalDirection)((random ?? Random.Shared).Next() % 4);
    }
    
    private bool CheckPosition(in Rect2I rectangle)
    {
        if (Position.X < 0 || Position.Y < 0 || Position.X >= rectangle.Width || Position.Y >= rectangle.Height)
        {
            return false;
        }
        return true;
    }
}