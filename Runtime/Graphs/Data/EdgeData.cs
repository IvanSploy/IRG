using System;

namespace IRG.Graphs
{
    [Serializable]
    public class EdgeData
    {
        public string FromNodeID;
        public string FromPortID;

        public string ToNodeID;
        //public string ToPortID; // (optional)
    }
}