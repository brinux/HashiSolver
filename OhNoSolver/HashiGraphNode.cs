namespace brinux.hashisolver
{
	public class HashiGraphNode
	{
		public HashiCellCoordinate SchemaCell { get; private set; }

		public List<HashiGraphConnection> Connections { get; private set; }
	}

	public class HashiGraphConnection
	{
		public List<HashiGraphNode> Nodes { get; private set; }

		public AxisEnum Axis { get; private set; }

		public int Weight { get; set; }
	}
}
