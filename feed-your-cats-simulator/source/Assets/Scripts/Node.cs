using UnityEngine;

public enum NodeValue {
    Emtpy,
    Floor,
    Connector
}

public class Node {
    public NodeValue Value { get; set; }
    public Vector2 WorldPosition { get; set; }
    public int Position { get; set; }

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost {
        get { return GCost + HCost; }
    }
    public Node Parent { get; set; }

    public Node(int position, Vector2 worldPosition) {
        WorldPosition = worldPosition;
        Position = position;
        Value = NodeValue.Emtpy;

        GCost = 0;
        HCost = 0;
    }
}