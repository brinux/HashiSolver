namespace brinux.hashisolver
{
    public struct HashiCellCoordinate
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public HashiCell Cell => _schema.Cells[Row][Column];

        public HashiSchema _schema { get; private set; }

        public HashiCellCoordinate(int row, int column, HashiSchema schema)
        {
            Row = row;
            Column = column;
            _schema = schema;
        }

        public bool CanProceed(DirectionEnum direction, int steps = 1)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    return Row - steps >= 0;
                case DirectionEnum.DOWN:
                    return Row + steps < _schema.Height;
                case DirectionEnum.LEFT:
                    return Column - steps >= 0;
                case DirectionEnum.RIGHT:
                    return Column + steps < _schema.Width;
            }

            throw new NotImplementedException($"Missing implementation for direction: {direction}");
        }

        public HashiCellCoordinate Move(DirectionEnum direction, int steps = 1)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    return new HashiCellCoordinate(Row - steps, Column, _schema);
                case DirectionEnum.DOWN:
                    return new HashiCellCoordinate(Row + steps, Column, _schema);
                case DirectionEnum.LEFT:
                    return new HashiCellCoordinate(Row, Column - steps, _schema);
                case DirectionEnum.RIGHT:
                    return new HashiCellCoordinate(Row, Column + steps, _schema);
            }

            throw new NotImplementedException($"Missing implementation for direction: {direction}");
        }

        public HashiCellCoordinate? MoveToNextValuedCell(DirectionEnum direction)
        {
            while (CanProceed(direction))
            {
                var cell = Move(direction);

                if (cell.Cell.IsValued)
                {
                    return cell;
                }
                else if (cell.Cell.IsEmpty || (cell.Cell.IsConnection && cell.Cell.ConnectionAxis == direction.GetAxis()))
                {
                    return cell.MoveToNextValuedCell(direction);
                }
                else
                {
                    break;
                }                
            }

            return null;
        }

		public Dictionary<DirectionEnum, int> CalculateCurrectConnections()
		{
			var result = new Dictionary<DirectionEnum, int>();

			foreach (var direction in Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>())
			{
				if (CanProceed(direction))
				{
					var movedCell = Move(direction);

					if (movedCell.Cell.IsConnection && movedCell.Cell.ConnectionAxis == direction.GetAxis())
					{
						result.Add(direction, movedCell.Cell.ConnectionWeight);
					}
				}
			}

			return result;
		}
	}
}
