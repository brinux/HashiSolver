namespace brinux.hashisolver
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var schema = new HashiSchema(18, 13, new List<HashiCellSetup>()
			{
                HashiCellSetup.SetupValuedCell(1, 1, 3),
                HashiCellSetup.SetupValuedCell(1, 4, 2),
                HashiCellSetup.SetupValuedCell(1, 6, 1),
                HashiCellSetup.SetupValuedCell(1, 9, 2),
                HashiCellSetup.SetupValuedCell(1, 11, 2),
                HashiCellSetup.SetupValuedCell(1, 13, 1),

                HashiCellSetup.SetupValuedCell(2, 2, 1),
                HashiCellSetup.SetupValuedCell(2, 5, 4),
                HashiCellSetup.SetupValuedCell(2, 7, 4),
                HashiCellSetup.SetupValuedCell(2, 10, 3),

                HashiCellSetup.SetupValuedCell(3, 1, 4),
                HashiCellSetup.SetupValuedCell(3, 3, 2),
                HashiCellSetup.SetupValuedCell(3, 6, 1),
                HashiCellSetup.SetupValuedCell(3, 9, 3),
                HashiCellSetup.SetupValuedCell(3, 11, 4),
                HashiCellSetup.SetupValuedCell(3, 13, 2),

                HashiCellSetup.SetupValuedCell(4, 10, 1),

				HashiCellSetup.SetupValuedCell(5, 1, 4),
				HashiCellSetup.SetupValuedCell(5, 5, 4),
				HashiCellSetup.SetupValuedCell(5, 9, 6),
				HashiCellSetup.SetupValuedCell(5, 11, 6),
				HashiCellSetup.SetupValuedCell(5, 13, 2),

				HashiCellSetup.SetupValuedCell(6, 10, 1),

				HashiCellSetup.SetupValuedCell(7, 1, 4),
				HashiCellSetup.SetupValuedCell(7, 3, 4),
				HashiCellSetup.SetupValuedCell(7, 7, 2),
                HashiCellSetup.SetupValuedCell(7, 9, 2),

				HashiCellSetup.SetupValuedCell(8, 2, 1),
                HashiCellSetup.SetupValuedCell(8, 4, 4),
				HashiCellSetup.SetupValuedCell(8, 10, 4),
                HashiCellSetup.SetupValuedCell(8, 13, 2),

                HashiCellSetup.SetupValuedCell(9, 1, 4),
                HashiCellSetup.SetupValuedCell(9, 3, 1),
				HashiCellSetup.SetupValuedCell(9, 6, 3),
                HashiCellSetup.SetupValuedCell(9, 8, 1),

				HashiCellSetup.SetupValuedCell(10, 11, 4),
				HashiCellSetup.SetupValuedCell(10, 13, 4),

				HashiCellSetup.SetupValuedCell(11, 1, 5),
				HashiCellSetup.SetupValuedCell(11, 4, 6),
				HashiCellSetup.SetupValuedCell(11, 6, 6),
				HashiCellSetup.SetupValuedCell(11, 9, 1),

				HashiCellSetup.SetupValuedCell(12, 8, 1),
				HashiCellSetup.SetupValuedCell(12, 10, 4),
				HashiCellSetup.SetupValuedCell(12, 12, 2),

				HashiCellSetup.SetupValuedCell(13, 1, 4),
				HashiCellSetup.SetupValuedCell(13, 3, 1),
				HashiCellSetup.SetupValuedCell(13, 6, 3),
				HashiCellSetup.SetupValuedCell(13, 9, 3),
				HashiCellSetup.SetupValuedCell(13, 11, 4),
				HashiCellSetup.SetupValuedCell(13, 13, 3),

				HashiCellSetup.SetupValuedCell(14, 2, 1),
				HashiCellSetup.SetupValuedCell(14, 4, 4),
				HashiCellSetup.SetupValuedCell(14, 8, 3),
				HashiCellSetup.SetupValuedCell(14, 12, 3),

				HashiCellSetup.SetupValuedCell(15, 1, 5),
				HashiCellSetup.SetupValuedCell(15, 5, 3),
				HashiCellSetup.SetupValuedCell(15, 7, 1),
				HashiCellSetup.SetupValuedCell(15, 9, 3),
				HashiCellSetup.SetupValuedCell(15, 11, 2),
				HashiCellSetup.SetupValuedCell(15, 13, 3),

				HashiCellSetup.SetupValuedCell(16, 10, 3),
				HashiCellSetup.SetupValuedCell(16, 12, 4),

				HashiCellSetup.SetupValuedCell(17, 1, 3),
				HashiCellSetup.SetupValuedCell(17, 3, 3),
				HashiCellSetup.SetupValuedCell(17, 6, 2),
				HashiCellSetup.SetupValuedCell(17, 9, 3),
				HashiCellSetup.SetupValuedCell(17, 11, 2),
				HashiCellSetup.SetupValuedCell(17, 13, 1),

				HashiCellSetup.SetupValuedCell(18, 2, 2),
				HashiCellSetup.SetupValuedCell(18, 5, 3),
				HashiCellSetup.SetupValuedCell(18, 8, 3),
				HashiCellSetup.SetupValuedCell(18, 10, 2),
				HashiCellSetup.SetupValuedCell(18, 12, 3)
			});

            HashiSchemaPrinter.PrintSchema(schema);

			var solver = new HashiSchemaSolver(schema);

			while (solver.Solve())
			{
                HashiSchemaPrinter.PrintSchema(schema);
             
				Console.ReadKey();
			}

			if (schema.IsSolved())
			{
				Console.WriteLine("Schema correctly solved");
			}
			else
			{
                Console.WriteLine("It was not possible to solve the schema");
			}
		}
	}
}