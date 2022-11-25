using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public sealed class Graph<T> : IEnumerable<GraphNode<T>>
	{
		readonly Dictionary<T, GraphNode<T>> nodes = new Dictionary<T, GraphNode<T>>();
		public int Count => nodes.Count;


		public GraphNode<T> Add(T vertex)
		{
			GraphNode<T> newNode = new GraphNode<T>(vertex);
			if (nodes.TryAdd(vertex, newNode))
				return newNode;
			else
				return default(GraphNode<T>);
		}

		public bool Contains(T vertex)
		{
			return nodes.ContainsKey(vertex);
		}

		public bool TryGetValue(T vertex, out GraphNode<T> result)
		{
			return nodes.TryGetValue(vertex, out result);
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<GraphNode<T>> GetEnumerator()
		{
			foreach(KeyValuePair<T,GraphNode<T>> item in nodes)
            {
				yield return item.Value;
            }
		}

		public void SetEdge(T from, T to, int weight, bool isBoth)
		{
			//GraphNode<T> fromNode;
			GraphNode<T> toNode;

			if (TryGetValue(from, out var fromNode) && TryGetValue(to, out toNode))
			{
				fromNode.AddEdge(toNode, weight);

				if (isBoth)
					toNode.AddEdge(fromNode, weight);
			}
			else
				throw new System.Exception("이 graph의 vertex가 아닙니다.");
		}

		public void SetEdge(T a, T b, int weight_ab, int weight_ba)
		{
			GraphNode<T> aNode;
			GraphNode<T> bNode;

			if (TryGetValue(a, out aNode) && TryGetValue(b, out bNode))
			{
				aNode.AddEdge(bNode, weight_ab);
				bNode.AddEdge(aNode, weight_ba);
			}
			else
				throw new System.Exception("이 graph의 vertex가 아닙니다.");
		}


		public GraphPath<T> CreatePath(T start, T end)
		{
			GraphNode<T> startNode;
			GraphNode<T> endNode;
			TryGetValue(start, out startNode);
			TryGetValue(end, out endNode);

			return new GraphPath<T>(startNode, endNode);
		}

		public List<GraphPath<T>> SearchAll(T start, T end, SearchPolicy policy)
		{
			GraphPath<T> path = CreatePath(start, end);
			List<GraphPath<T>> paths = new List<GraphPath<T>>();
			FindPath(start, end, path, paths);

			return paths;
		}

		public void FindPath(T start, T end, GraphPath<T> path, List<GraphPath<T>> paths)
        {
			if (Equals(start, end))
            {
				paths.Add(path.Clone());
				return;
			}
				

			GraphNode<T> startNode;
			TryGetValue(start, out startNode);
			// log 필요

			foreach (KeyValuePair<T,int> edges in startNode)
            {
				if (!path.IsVisited(edges.Key))
                {
					path.Vertexs.Add(edges.Key);
					FindPath(edges.Key, end, path, paths);
					path.Vertexs.Remove(edges.Key);
				}
            }
        }

		/*
		public bool Remove(T vertex)
		{

		}

		public void Clear()
		{

		}
		*/

		public enum SearchPolicy
		{
			Visit = 0,
			Pass,
		}
	}

	public class GraphNode<T> : IEnumerable<KeyValuePair<T, int>>
	{
		public T ThisVertex;
		public List<Edge> Edges = new List<Edge>();

		public struct Edge
		{
			//public T Vertex;
			public GraphNode<T> Node;
			public int Weight;

			public Edge(GraphNode<T> node, int weight)
			{
				Node = node;
				Weight = weight;
			}
		}

		public GraphNode(T vertex) {
			ThisVertex = vertex;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<KeyValuePair<T, int>> GetEnumerator()
		{
			foreach(Edge edge in Edges)
            {
				yield return new KeyValuePair<T, int>(edge.Node.ThisVertex, edge.Weight);
            }
		}

		public void AddEdge(GraphNode<T> node, int weight)
		{
			Edges.Add(new Edge(node, weight));
		}

		public bool TryGetValue(T vertex, out Edge edge)
		{
			foreach (Edge e in Edges)
			{
				if (EqualityComparer<T>.Default.Equals(e.Node.ThisVertex, vertex))
                {
					edge = e;
					return true;
				}
			}
            edge = default;
            return false;
		}
	}

	public class GraphPath<T> : IEnumerable<T>
	{
		public GraphNode<T> Start { private set; get; } = null;
		public GraphNode<T> End { private set; get; } = null;

		public readonly List<T> Vertexs = new List<T>();
		public int Count => Vertexs.Count;
		public bool IsNoWay
		{
			get
			{
				return !Equals(Vertexs[Count - 1], End.ThisVertex);
			}
		}

		public GraphPath(GraphNode<T> start, GraphNode<T> end){
			Start = start;
			End = end;
			Vertexs.Add(start.ThisVertex);
		}

		public int GetTotalWeight()
		{
			return 10;
		}

		public bool IsVisited(T vertex)
		{
			return Vertexs.Contains(vertex);
		}

		public bool IsPassed(T vertex)
		{
			return IsPassed(End.ThisVertex, vertex);
		}

		public bool IsPassed(T from, T to)
		{
			for(int i = 1; i < Vertexs.Count; i++)
            {
				if (EqualityComparer<T>.Default.Equals(Vertexs[i], to)
					&& EqualityComparer<T>.Default.Equals(Vertexs[i - 1], from))
					return true;
            }
			return false;
		}

		public GraphPath<T> Clone()
		{
			GraphPath<T> returnPath = new GraphPath<T>(Start, End);
			foreach(T t in Vertexs)
            {
				returnPath.Vertexs.Add(t);
            }
			return returnPath;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{
			foreach (T t in Vertexs)
            {
				yield return t;
            }
		}
	}
}