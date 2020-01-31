using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UndirGraphTag<T, T2> : Graph<T>
{
    public UndirGraphTag()
    {
        nodes = new List<Node<T>>();
        edges = new List<Edge<T>>();
    }

    public override void DeleteEdge(Edge<T> edge)
    {
        Node<T> from = edge.From;
        Node<T> to = edge.To;

        for (int i = 0; i < edges.Count; i++)
        {
            if ((edges[i].To == to && edges[i].From == from) || (edges[i].To == from && edges[i].From == to))
            {
                edges.RemoveAt(i);
                i--;
            }
        }
    }

    public override void AddEdge(Node<T> from, Node<T> to)
    {
        EdgeTag<T, T2> newEdge = new EdgeTag<T, T2>(from, to);
        EdgeTag<T, T2> backEdge = new EdgeTag<T, T2>(to, from);
        if (!edges.Contains(newEdge))
        {
            edges.Add(newEdge);
        }
        if (!edges.Contains(backEdge))
        {
            edges.Add(backEdge);
        }
    }

    public void AddEdge(Node<T> from, Node<T> to, T2 tag)
    {
        EdgeTag<T, T2> newEdge = new EdgeTag<T, T2>(from, to);
        EdgeTag<T, T2> backEdge = new EdgeTag<T, T2>(to, from);
        newEdge.Tag = tag;
        backEdge.Tag = tag;
        if (!edges.Contains(newEdge))
        {
            edges.Add(newEdge);
        }
        if (!edges.Contains(backEdge))
        {
            edges.Add(backEdge);
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
