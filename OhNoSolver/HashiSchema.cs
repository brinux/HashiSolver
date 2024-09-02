namespace brinux.hashisolver
{
	public class HashiSchema
	{
		public HashiCell[][] Cells { get; private set; }

		public int Height { get; private set; }
		public int Width { get; private set; }

        public HashiSchema(int height, int width)
		{
			Height = height;
			Width = width;

			Cells = new HashiCell[Height][];

			for (int r = 0; r < Height; r++)
			{
				Cells[r] = new HashiCell[Width];

				for (int c = 0; c < Width; c++)
				{
					Cells[r][c] = new HashiCell();
				}
			}
		}

		public HashiSchema(int height, int width, List<HashiCellSetup> setup) : this(height, width)
		{
			foreach (var cell in setup)
			{
				if (cell.Row >= Height)
				{
					throw new ArgumentException($"The setup for cell at { cell.Row }:{ cell.Column } is out of the configured schema ({ Height }x{ Width }).");
				}
				else if (cell.Column >= Width)
				{
					throw new ArgumentException($"The setup for cell at { cell.Row }:{ cell.Column } is out of the configured schema ({ Height }x{ Width }).");
				}

				switch (cell.Status)
				{
					case HashiCellStatusEnum.Valued:
						Cells[cell.Row][cell.Column] = new HashiCell(cell.Value);
						break;

					case HashiCellStatusEnum.Connection:
						throw new ArgumentException("Connections cannot be setup.");

					default:
						break;
				}
			}
		}

		public bool IsSolved()
		{
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    if (Cells[r][c].IsValued && !Cells[r][c].IsSolved)
                    {
						return false;
                    }
                }
            }

			return true;
        }
    }
}