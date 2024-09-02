namespace brinux.hashisolver
{
	public class HashiCell
	{
		public HashiCellStatusEnum Status { get; set; } = HashiCellStatusEnum.Empty;

		public int Value { get; private set; } = 0;
		public int ConnectionWeight { get; set; } = 0;

		public bool IsSolved { get; set; } = false;

		public AxisEnum? ConnectionAxis { get; set; }

		public int? ConnectionsGroup { get; set; } = null;

		public bool IsEmpty => Status == HashiCellStatusEnum.Empty;
		public bool IsConnection => Status == HashiCellStatusEnum.Connection;
		public bool IsValued => Status == HashiCellStatusEnum.Valued;

		public HashiCell()
		{
			Status = HashiCellStatusEnum.Empty;
		}

		public HashiCell(int value)
		{
			if (value < 1 || value > 8)
			{
				throw new ArgumentOutOfRangeException($"Value { value} for cell is out of range 1-8.");
			}

			Status = HashiCellStatusEnum.Valued;
			Value = value;
		}
	}
}
