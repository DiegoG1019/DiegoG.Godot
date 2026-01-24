using System.Collections;
using Godot;

namespace DiegoG.Godot.Common;

public enum GridPositionOffset
{
    None = 0,
    Center = 1,
    CellSize = 2
}

public readonly record struct BoundedSquareGrid(SquareGrid Grid, int XCells, int YCells)
{
    public Vector2 this[int x, int y]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(x, XCells);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(y, YCells);
            ArgumentOutOfRangeException.ThrowIfNegative(x);
            ArgumentOutOfRangeException.ThrowIfNegative(y);

            return Grid.GetPosition(x, y);
        }
    }

    public Vector4 GetArea(
        int x1, int y1, int x2, int y2,
        GridPositionOffset x1offset = GridPositionOffset.None,
        GridPositionOffset y1offset = GridPositionOffset.None,
        GridPositionOffset x2offset = GridPositionOffset.CellSize,
        GridPositionOffset y2offset = GridPositionOffset.CellSize
    )
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(x1, XCells);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(x2, XCells);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(y1, YCells);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(y2, YCells);
        ArgumentOutOfRangeException.ThrowIfNegative(x1);
        ArgumentOutOfRangeException.ThrowIfNegative(y1);
        ArgumentOutOfRangeException.ThrowIfNegative(x2);
        ArgumentOutOfRangeException.ThrowIfNegative(y2);

        return Grid.GetArea(x1, y1, x2, y2, x1offset, y1offset, x2offset, y2offset);
    }

    public Vector2 GetPosition(int x, int y, GridPositionOffset xoffset = GridPositionOffset.None, GridPositionOffset yoffset = GridPositionOffset.None)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(x, XCells);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(y, YCells);
        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);

        return Grid.GetPosition(x, y, xoffset, yoffset);
    }

    public GridCellsEnumerable GetCells(GridPositionOffset xoffset = GridPositionOffset.None,
        GridPositionOffset yoffset = GridPositionOffset.None)
        => new GridCellsEnumerable(this, xoffset, yoffset);

    public int TotalCells => XCells * YCells;
    public float TotalArea => XCells * Grid.XScale * YCells * Grid.YScale;
    public Rect2 TotalAreaRectangle => new Rect2(0, 0, XCells * Grid.XScale, YCells * Grid.YScale);
    public Rect2I TotalCellsRectangle => new Rect2I(0, 0, XCells, YCells);

    public int CompareHorizontalBounds(int x) => x < 0 ? -1 : x >= XCells ? 1 : 0;
    public int CompareVerticalBounds(int y) => y < 0 ? -1 : y >= YCells ? 1 : 0;
    
    public bool IsWithinHorizontalBounds(int x) => x >= 0 && x < XCells;
    public bool IsWithinVerticalBounds(int y) => y >= 0 && y < YCells;
    public bool IsWithinBounds(int x, int y) => IsWithinHorizontalBounds(x) && IsWithinVerticalBounds(y);
    
    public GridRectanglesEnumerable GetCellRectangles()
        => new GridRectanglesEnumerable(this);

    public GridRectanglesIEnumerable GetCellIntRectangles()
        => new GridRectanglesIEnumerable(this);
}

public readonly record struct SquareGrid(float XScale, float YScale)
{
    public SquareGrid(float scale) : this(scale, scale)
    { }

    public Vector2 this[int x, int y]
        => GetPosition(x, y);

    public Vector4 GetArea(
        int x1, int y1, int x2, int y2,
        GridPositionOffset x1offset = GridPositionOffset.None,
        GridPositionOffset y1offset = GridPositionOffset.None,
        GridPositionOffset x2offset = GridPositionOffset.CellSize,
        GridPositionOffset y2offset = GridPositionOffset.CellSize
    )
        => GetArea(x1, y1, x2, y2, XScale, YScale, x1offset, y1offset, x2offset, y2offset);

    public Vector2 GetPosition(int x, int y, GridPositionOffset xoffset = GridPositionOffset.None, GridPositionOffset yoffset = GridPositionOffset.None)
        => GetPosition(x, y, XScale, YScale, xoffset, yoffset);

    public float GetXOffset(GridPositionOffset offset)
        => GetOffset(offset, XScale);

    public float GetYOffset(GridPositionOffset offset)
        => GetOffset(offset, YScale);

    // ----------- static

    public static Vector4 GetArea(
        int x1, int y1, int x2, int y2,
        float xscale, float yscale,
        GridPositionOffset x1offset = GridPositionOffset.None,
        GridPositionOffset y1offset = GridPositionOffset.None,
        GridPositionOffset x2offset = GridPositionOffset.CellSize,
        GridPositionOffset y2offset = GridPositionOffset.CellSize
    )
        => new(
            x1 * xscale + GetOffset(x1offset, xscale),
            y1 * yscale + GetOffset(y1offset, yscale),
            x2 * xscale + GetOffset(x2offset, xscale),
            y2 * yscale + GetOffset(y2offset, yscale)
        );

    public static Vector2 GetPosition(
        int x, int y, float xscale, float yscale,
        GridPositionOffset xoffset = GridPositionOffset.None, GridPositionOffset yoffset = GridPositionOffset.None
    )
        => new(x * xscale + GetOffset(xoffset, xscale), y * yscale + GetOffset(yoffset, yscale));

    public static float GetOffset(GridPositionOffset offset, float scale)
        => offset switch
        {
            GridPositionOffset.None => 0,
            GridPositionOffset.Center => scale / 2f,
            GridPositionOffset.CellSize => scale,
            _ => throw new ArgumentException($"Unknown GridPositionOffset: {offset}")
        };
}

public struct GridRectanglesIEnumerable(BoundedSquareGrid grid) : IEnumerator<Rect2I>
{
    private int x;
    private int y;
    
    public bool MoveNext()
    {
        if (y >= 100)
        {
            x++;
            y = 0;
        }

        if (x >= 100) return false;

        Current = new Rect2I(
            (int)(x * grid.Grid.XScale),
            (int)(y++ * grid.Grid.YScale),
            (int)(grid.Grid.XScale),
            (int)(grid.Grid.YScale)
        );
        
        return true;
    }

    public void Reset()
    {
        x = 0;
        y = 0;
    }

    public Rect2I Current { get; private set; }
    
    object? IEnumerator.Current => Current;

    public void Dispose(){}

    public GridRectanglesIEnumerable GetEnumerator() => this;
}

public struct GridRectanglesEnumerable(BoundedSquareGrid grid) : IEnumerator<Rect2>
{
    private int x;
    private int y;
    
    public bool MoveNext()
    {
        if (y >= 100)
        {
            x++;
            y = 0;
        }

        if (x >= 100) return false;

        Current = new Rect2(
            x * grid.Grid.XScale,
            y++ * grid.Grid.YScale,
            grid.Grid.XScale,
            grid.Grid.YScale
        );
        
        return true;
    }

    public void Reset()
    {
        x = 0;
        y = 0;
    }

    public Rect2 Current { get; private set; }
    
    object? IEnumerator.Current => Current;

    public void Dispose(){}

    public GridRectanglesEnumerable GetEnumerator() => this;
}

public struct GridCellsEnumerable(BoundedSquareGrid grid, GridPositionOffset xOffset, GridPositionOffset yOffset) : IEnumerator<Vector2>
{
    private int x;
    private int y;
    
    public bool MoveNext()
    {
        if (y >= 100)
        {
            x++;
            y = 0;
        }

        if (x >= 100) return false;

        Current = grid.GetPosition(x, y++, xOffset, yOffset);
        return true;
    }

    public void Reset()
    {
        x = 0;
        y = 0;
    }

    public Vector2 Current { get; private set; }
    
    object? IEnumerator.Current => Current;

    public void Dispose(){}

    public GridCellsEnumerable GetEnumerator() => this;
}
