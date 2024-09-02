namespace brinux.hashisolver
{
	public class HashiSchemaSolver
	{
		private HashiSchema _schema;
		private HashiCellSolver _cellSolver;

		public HashiSchemaSolver(HashiSchema schema)
		{
			_schema = schema;
			_cellSolver = new HashiCellSolver();
		}

		// This is just to enable mocked tests without introducing D.I.; sorry for the lazyness! :D
        public HashiSchemaSolver(HashiSchema schema, HashiCellSolver cellSolver)
        {
            _schema = schema;
            _cellSolver = cellSolver;
        }

        public bool Solve()
		{
			if (!_schema.IsSolved())
			{
				for (int r = 0; r < _schema.Height; r++)
				{
					for (int c = 0; c < _schema.Width; c++)
					{
						if (_schema.Cells[r][c].IsValued && !_schema.Cells[r][c].IsSolved)
						{
							var cell = new HashiCellCoordinate(r, c, _schema);

							var newConnections = _cellSolver.SolveCell(cell);

							if (newConnections.Any(d => d.ConnectionsNumber > 0))
							{
								ApplyConnections(newConnections, cell);

								RegisterImpactedNodes(newConnections, cell);

								return true;
							}
						}
					}
				}
			}

			return false;
		}

		private void ApplyConnections(List<HashiCandidateConnection> connections, HashiCellCoordinate cell)
		{
			foreach (var connection in connections)
			{
				var currentCell = cell.Move(connection.Direciton);

				while (!currentCell.Cell.IsValued)
				{
					currentCell.Cell.Status = HashiCellStatusEnum.Connection;
					currentCell.Cell.ConnectionAxis = connection.Direciton.GetAxis();
					currentCell.Cell.ConnectionWeight += connection.ConnectionsNumber;

					currentCell = currentCell.Move(connection.Direciton);

					if (cell.CalculateCurrectConnections().Sum(c => c.Value) == cell.Cell.Value)
					{
						cell.Cell.IsSolved = true;
					}
				}
			}
		}

		private void RegisterImpactedNodes(List<HashiCandidateConnection> connections, HashiCellCoordinate cell)
		{

		}
	}
}