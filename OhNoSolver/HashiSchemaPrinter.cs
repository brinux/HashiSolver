namespace brinux.hashisolver
{
	public class HashiSchemaPrinter
	{
		private const string SINGLE_HORIZONTAL_CONNECTION = "─";
		private const string DOUBLE_HORIZONTAL_CONNECTION = "═";
		private const string SINGLE_VERTICAL_CONNECTION = "│";
		private const string DOUBLE_VERTICAL_CONNECTION = "║";

		private const string UP_LEFT_CORNER = "┌";
		private const string UP_RIGHT_CORNER = "┐";
		private const string BOTTOM_LEFT_CORNER = "└";
		private const string BOTTOM_RIGHT_CORNER = "┘";
		private const string LEFT_SIDE = "├";
		private const string RIGHT_SIDE = "┤";
		private const string TOP_SIDE = "┬";
		private const string BOTTOM_SIDE = "┴";
		private const string CROSS_CONNECTION = "┼";

		public static void PrintSchema(HashiSchema schema)
		{
			if (schema == null)
			{
				throw new NullReferenceException("The schema is undefined");
			}

			for (int r = 0; r < schema.Height; r++)
			{
				for (int c = 0; c < schema.Width; c++)
				{
					switch (schema.Cells[r][c].Status)
					{
						case HashiCellStatusEnum.Valued:
							Console.ForegroundColor = schema.Cells[r][c].IsSolved ?
								ConsoleColor.Green :
								ConsoleColor.White;

							Console.Write(schema.Cells[r][c].Value);
							break;

						case HashiCellStatusEnum.Connection:
							Console.ForegroundColor = ConsoleColor.White;

							Console.Write(
								schema.Cells[r][c].ConnectionAxis == AxisEnum.UP_DOWN ?
									schema.Cells[r][c].ConnectionWeight == 1 ?
										SINGLE_VERTICAL_CONNECTION :
										DOUBLE_VERTICAL_CONNECTION :
									schema.Cells[r][c].ConnectionWeight == 1 ?
										SINGLE_HORIZONTAL_CONNECTION :
										DOUBLE_HORIZONTAL_CONNECTION);
							break;

						default:
							Console.ForegroundColor = ConsoleColor.DarkGray;

							if (r == 0)
							{
								if (c == 0)
								{
									Console.Write(UP_LEFT_CORNER);
								}
								else if (c == schema.Width - 1)
								{
									Console.Write(UP_RIGHT_CORNER);
								}
								else
								{
									Console.Write(TOP_SIDE);
								}
							}
							else if (r == schema.Height - 1)
							{
								if (c == 0)
								{
									Console.Write(BOTTOM_LEFT_CORNER);
								}
								else if (c == schema.Width - 1)
								{
									Console.Write(BOTTOM_RIGHT_CORNER);
								}
								else
								{
									Console.Write(BOTTOM_SIDE);
								}
							}
							else
							{
								if (c == 0)
								{
									Console.Write(LEFT_SIDE);
								}
								else if (c == schema.Width - 1)
								{
									Console.Write(RIGHT_SIDE);
								}
								else
								{
									Console.Write(CROSS_CONNECTION);
								}
							}							
							break;
					}

					if (c < schema.Width - 1)
					{
						if ((schema.Cells[r][c + 1].IsConnection && schema.Cells[r][c + 1].ConnectionAxis == AxisEnum.LEFT_RIGHT) ||
							(schema.Cells[r][c].IsConnection && schema.Cells[r][c].ConnectionAxis == AxisEnum.LEFT_RIGHT))
						{
							Console.ForegroundColor = ConsoleColor.White;

							Console.Write(schema.Cells[r][c + 1].ConnectionWeight == 1 || schema.Cells[r][c].ConnectionWeight == 1 ?
								SINGLE_HORIZONTAL_CONNECTION + SINGLE_HORIZONTAL_CONNECTION :
								DOUBLE_HORIZONTAL_CONNECTION + DOUBLE_HORIZONTAL_CONNECTION);
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.DarkGray;
							
							Console.Write(SINGLE_HORIZONTAL_CONNECTION + SINGLE_HORIZONTAL_CONNECTION);
						}
					}
				}

				Console.WriteLine();

				if (r + 1 < schema.Height)
				{
					for (int c = 0; c < schema.Width; c++)
					{
						if ((schema.Cells[r+1][c].IsConnection && schema.Cells[r + 1][c].ConnectionAxis == AxisEnum.UP_DOWN) ||
							(schema.Cells[r][c].IsConnection && schema.Cells[r][c].ConnectionAxis == AxisEnum.UP_DOWN))
						{
							Console.ForegroundColor = ConsoleColor.White;

							Console.Write((schema.Cells[r + 1][c].IsConnection && schema.Cells[r + 1][c].ConnectionWeight == 1) ||
							(schema.Cells[r][c].IsConnection && schema.Cells[r][c].ConnectionWeight == 1) ?
								SINGLE_VERTICAL_CONNECTION :
								DOUBLE_VERTICAL_CONNECTION);
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.DarkGray;

							Console.Write(SINGLE_VERTICAL_CONNECTION);
						}

						Console.Write("  ");
					}
				}

				Console.WriteLine();
			}

			Console.WriteLine();
		}
	}
}
