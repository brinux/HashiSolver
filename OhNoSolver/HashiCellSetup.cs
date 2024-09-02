namespace brinux.hashisolver
{
	public class HashiCellSetup
	{
		public int Row { get; set; }
		public int Column { get; set; }
		public HashiCellStatusEnum Status { get; set; }
		public int Value { get; set; }

		private HashiCellSetup() {}

		public static HashiCellSetup SetupValuedCell(int row, int column, int value)
		{
			if (value <= 0 || value > 8)
			{
				throw new ArgumentException("A full cell can only be set up with a positive value in the range 1-8.");
			}

			return new HashiCellSetup()
			{
				Row = row - 1,
				Column = column - 1,
				Status = HashiCellStatusEnum.Valued,
				Value = value
			};
		}
	}
}
