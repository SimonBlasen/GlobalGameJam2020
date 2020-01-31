using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirGraph<T> : Graph<T>
{
    public DirGraph()
    {
        nodes = new List<Node<T>>();
        edges = new List<Edge<T>>();
    }

    public override void AddEdge(Node<T> from, Node<T> to)
    {
        Edge<T> newEdge = new Edge<T>(from, to);
        if (edges.Contains(newEdge) == false)
        {
            edges.Add(newEdge);
        }
    }

    public override bool AddNode(T node)
    {
        Node<T> newNode = new Node<T>(node);
        if (!nodes.Contains(newNode))
        {
            nodes.Add(newNode);
            return true;
        }

        return false;
    }
}
