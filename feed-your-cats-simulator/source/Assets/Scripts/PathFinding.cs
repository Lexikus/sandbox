using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PathFinding {

	public static List<Node> Path { get; private set; }

	public static void Find(Vector2 startPos, Vector2 targetPos) {
		World world = World.Instance;

		Node startNode = world.GetNodeFromWorldPosition(startPos);
		Node targetNode = world.GetNodeFromWorldPosition(targetPos);

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i++) {
				if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost) {
					if (openSet[i].HCost < node.HCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node.Position == targetNode.Position) {
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (Node neighbour in world.GetNeighbours(node.Position)) {
				if (neighbour.Value == NodeValue.Floor || closedSet.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = node.GCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour)) {
					neighbour.GCost = newCostToNeighbour;
					neighbour.HCost = GetDistance(neighbour, targetNode);
					neighbour.Parent = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
	}

	private static void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.Parent;
		}
		path.Reverse();
		Path = path;
	}

	private static int GetDistance(Node nodeA, Node nodeB) {
		Vector2 vecA = nodeA.WorldPosition;
		Vector2 vecB = nodeB.WorldPosition;

		int dstX = Mathf.Abs((int)(vecA.x - vecB.x));
		int dstY = Mathf.Abs((int)(vecB.y - vecB.y));

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);

		return 14 * dstX + 10 * (dstY - dstX);
	}
}