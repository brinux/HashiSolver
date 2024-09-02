namespace brinux.hashisolver
{
    public class HashiCandidateConnection
    {
        public DirectionEnum Direciton { get; set; }
        public int ConnectionsNumber { get; set; }

        public HashiCandidateConnection(DirectionEnum direction, int connectionsNumber)
        {
            Direciton = direction;
            ConnectionsNumber = connectionsNumber;
        }
    }
}
