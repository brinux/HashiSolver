namespace brinux.hashisolver
{
    public class HashiCandidateMove
    {
        public HashiCellCoordinate CellCoordinate { get; set; }
        public List<HashiCandidateConnection> Connections { get; set; } = new List<HashiCandidateConnection>();

        public HashiCandidateMove(HashiCellCoordinate cell, List<HashiCandidateConnection> connections)
        {
            CellCoordinate = cell;
            Connections = connections; ;
        }
    }
}
