using BattleShip.Common.Enums;

namespace BattleShip.Application.Objects.Field;

public class BattleField
{
    private const int SHIP_MAX_SIZE = 4;
    private const int SHIP_COUNT = 4;

    public int Size { get; set; }

    public CellType[][] Field { get; set; } = null!;

    public bool FillingIsCompleted { get; set; }

    public BattleField(int size)
    {
        Size = size;
    }

    public void PrepareField()
    {
        Field = new CellType[Size][];

        for (var i = 0; i < Size; i++)
        {
            Field[i] = new CellType[Size];
            for (var j = 0; j < Size; j++)
            {
                Field[i][j] = CellType.Empty;
            }
        }
    }

    public void GenerateField()
    {
        for (var shipSize = SHIP_MAX_SIZE; shipSize > 0; shipSize--)
        {
            for (var shipNumber = GetShipNumber(shipSize); shipNumber > 0; shipNumber--)
            {
                AddShipToField(shipSize);
            }
        }        
    }    

    private void AddShipToField(int shipSize)
    {
        while (true)
        {
            var lineIndex = GetRandomIndex();
            var startCellIndex = 0;
            if (CanAddToLine(lineIndex, shipSize, ref startCellIndex))
            {
                AddShipToLine(lineIndex, shipSize, startCellIndex);
                return;
            }

            RotateShip();
            startCellIndex = 0;
            if (CanAddToLine(lineIndex, shipSize, ref startCellIndex))
            {
                AddShipToLine(lineIndex, shipSize, startCellIndex);
                return;
            }
        }
    }

    private bool CanAddToLine(int lineIndex, int shipSize, ref int startCellIndex)
    {
        var cellIndexRange = Enumerable.Range(0, Size).ToArray();
        Shuffle(cellIndexRange);

        foreach (var initiateCellIndex in cellIndexRange)
        {
            for (var cellIndex = initiateCellIndex; cellIndex < Size; cellIndex++)
            {                
                var reservedFieldsCount = GetReservedFieldsCount(cellIndex, shipSize);

                if (IsLineOverloaded(cellIndex, reservedFieldsCount))
                {
                    break;
                }

                var emptyCells = GetCountOfEmptyCells(lineIndex, cellIndex, shipSize, reservedFieldsCount);

                if (TryCheckIfCellsEnough(cellIndex, emptyCells, shipSize, ref startCellIndex))
                {
                    return true;
                }
                
            }
        }

        startCellIndex = -1;
        return false;
    }

    private int GetCountOfEmptyCells(int lineIndex, int cellIndex, int shipSize, int reservedFieldsCount)
    {
        var emptyCells = 0;
        for (var blockOfShip = 0; blockOfShip < reservedFieldsCount; blockOfShip++)
        {
            var nextCellToCheck = cellIndex + blockOfShip;
            if (lineIndex == 0)
            {
                if (CheckZeroLine(shipSize, cellIndex, nextCellToCheck))
                {
                    emptyCells++;                    
                }
                continue;
            }

            if (lineIndex == Size - 1)
            {
                if (CheckLastLine(shipSize, cellIndex, nextCellToCheck))
                {
                    emptyCells++;                    
                }
                continue;
            }

            if (CheckMeddleLine(lineIndex, shipSize, cellIndex, nextCellToCheck))
            {
                emptyCells++;
            }
        }

        return emptyCells;
    }

    private bool TryCheckIfCellsEnough(int cellIndex, int emptyCells, int shipSize, ref int startCellIndex)
    {
        if (cellIndex == 0 || cellIndex == Size - shipSize - 1)
        {
            if (emptyCells == shipSize + 1)
            {
                startCellIndex = cellIndex;
                return true;
            }
        }

        if (emptyCells == shipSize + 2)
        {
            startCellIndex = cellIndex;
            return true;
        }

        return false;
    }

    private int GetReservedFieldsCount(int cellIndex, int shipSize)
    {
        var reservedFieldsCount = shipSize + 2;
        if (cellIndex == 0 || cellIndex == Size - shipSize - 1)
        {
            reservedFieldsCount = shipSize + 1;
        }

        return reservedFieldsCount;
    }

    private bool CheckZeroLine(int shipSize, int cellIndex, int nextCellToCheck)
    {
        if (Field[0][nextCellToCheck] != CellType.Ship
        && Field[1][nextCellToCheck] != CellType.Ship) 
        {
            if (cellIndex != 0 && Field[0][cellIndex - 1] != CellType.Ship
                && Field[1][cellIndex - 1] != CellType.Ship)    
            {
                return true;
            }

            if (cellIndex == 0) 
            {
                return true;
            }
        }


        return false;
    }

    private bool IsLineOverloaded(int cellIndex, int reservedFieldsCount) =>
        cellIndex + reservedFieldsCount > Size - 1
                    && cellIndex + reservedFieldsCount - 1 > Size - 1;

    private bool CheckLastLine(int shipSize, int cellIndex, int nextCellToCheck)
    {

        if (Field[Size - 1][nextCellToCheck] != CellType.Ship
        && Field[Size - 2][nextCellToCheck] != CellType.Ship)
        {
            if (cellIndex != 0 && Field[Size - 1][cellIndex - 1] != CellType.Ship
                && Field[Size - 2][cellIndex - 1] != CellType.Ship)
            {
                return true;
            }
            if (cellIndex == 0)  
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckMeddleLine(int lineIndex, int shipSize, int cellIndex, int nextCellToCheck)
    {

        if (Field[lineIndex - 1][nextCellToCheck] != CellType.Ship
        && Field[lineIndex][nextCellToCheck] != CellType.Ship
        && Field[lineIndex + 1][nextCellToCheck] != CellType.Ship)
        {
            if (cellIndex != 0 && Field[lineIndex - 1][cellIndex - 1] != CellType.Ship
        && Field[lineIndex][cellIndex - 1] != CellType.Ship
        && Field[lineIndex + 1][cellIndex - 1] != CellType.Ship)
            {
                return true;
            }

            if (cellIndex == 0)  
            {
                return true;
            }
        }
        return false;
    }

    private void AddShipToLine(int lineIndex, int shipSize, int startCellIndex)
    {
        var reservedFieldsCount = GetReservedFieldsCount(startCellIndex, shipSize);
        
        if (IsLineOverloaded(startCellIndex, reservedFieldsCount))
        {
            throw new ArgumentException($"Invalid index startCellIndex + reservedFieldsCount = {startCellIndex + reservedFieldsCount} > Size - 1");
        }

        for (var cellIndex = startCellIndex; cellIndex < startCellIndex + reservedFieldsCount; cellIndex++)
        {
            if(AddCellToLineWithNotZeroCell(cellIndex, lineIndex, startCellIndex, shipSize, reservedFieldsCount))
            {              
                continue;
            }

            AddCellToLine(lineIndex, shipSize, cellIndex);
        }
    }

    private bool AddCellToLineWithNotZeroCell(int cellIndex, int lineIndex, int startCellIndex, int shipSize, int reservedFieldsCount)
    {
        if (CheckBeginningOfShip(cellIndex, startCellIndex)
                || CheckEndingOfShip(cellIndex, startCellIndex, reservedFieldsCount)
                || CheckEndOfShipInTheEndOfLine(cellIndex, startCellIndex, reservedFieldsCount, shipSize))
        {
            if (lineIndex == 0)
            {
                AddShipToZeroLineNotZeroIndex(shipSize, cellIndex);
                return true;
            }

            if (lineIndex == Size - 1)
            {
                AddShipToLastLineNotZeroCell(shipSize, cellIndex);
                return true;
            }

            AddShipToMiddleLineNotZeroCell(lineIndex, shipSize, cellIndex);
            return true;
        }

        return false;
    }

    private bool CheckBeginningOfShip(int cellIndex, int startCellIndex) =>
        cellIndex != 0 && cellIndex == startCellIndex;

    private bool CheckEndingOfShip(int cellIndex, int startCellIndex, int reservedFieldsCount) =>
        cellIndex != Size - 1 && cellIndex == startCellIndex + reservedFieldsCount - 1;

    private bool CheckEndOfShipInTheEndOfLine(int cellIndex, int startCellIndex, int reservedFieldsCount, int shipSize) =>
        cellIndex == Size - 1 && cellIndex == startCellIndex + reservedFieldsCount - 1 && reservedFieldsCount == shipSize + 2;

    private void AddCellToLine(int lineIndex, int shipSize, int cellIndex)
    {
        if (lineIndex == 0)
        {
            AddShipToZeroLine(shipSize, cellIndex);            
            return;
        }

        if (lineIndex == Size - 1)
        {
            AddShipToLastLine(shipSize, cellIndex);            
            return;
        }

        AddShipToMiddleLine(lineIndex, shipSize, cellIndex);        
    }

    private void AddShipToZeroLineNotZeroIndex(int shipSize, int cellIndex)
    {        
        Field[0][cellIndex] = CellType.Forbidden;    
        Field[1][cellIndex] = CellType.Forbidden;
    }

    private void AddShipToZeroLine(int shipSize, int cellIndex)
    {        
        Field[0][cellIndex] = CellType.Ship;     
        Field[1][cellIndex] = CellType.Forbidden;
    }

    private void AddShipToLastLineNotZeroCell(int shipSize, int cellIndex)
    {        
        Field[Size - 1][cellIndex] = CellType.Forbidden;     
        Field[Size - 2][cellIndex] = CellType.Forbidden;
    }

    private void AddShipToLastLine(int shipSize, int cellIndex)
    {        
        Field[Size - 1][cellIndex] = CellType.Ship;     
        Field[Size - 2][cellIndex] = CellType.Forbidden;
    }

    private void AddShipToMiddleLineNotZeroCell(int lineIndex, int shipSize, int cellIndex)
    {        
        Field[lineIndex - 1][cellIndex] = CellType.Forbidden;     
        Field[lineIndex][cellIndex] = CellType.Forbidden;        
        Field[lineIndex + 1][cellIndex] = CellType.Forbidden;
    }

    private void AddShipToMiddleLine(int lineIndex, int shipSize, int cellIndex)
    {        
        Field[lineIndex - 1][cellIndex] = CellType.Forbidden;     
        Field[lineIndex][cellIndex] = CellType.Ship;        
        Field[lineIndex + 1][cellIndex] = CellType.Forbidden;
    }

    private int GetShipNumber(int shipSize) =>
        SHIP_COUNT - shipSize + 1;

    private int GetRandomIndex()
    {
        var random = new Random(DateTime.Now.Microsecond);

        return random.Next(Size);
    }

    private void Shuffle(int[] array)
    {
        Random random = new Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    public void RotateShip()
    {
        CellType[][] newMatrix = new CellType[Field.Length][];
        var rows = Field.Length;
        var cols = Field[0].Length;

        for (var i = 0; i < Size; i++)
        {
            newMatrix[i] = new CellType[Size];
        }

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newMatrix[j][rows - 1 - i] = Field[i][j];
            }
        }
        Field = newMatrix;
    }
}
