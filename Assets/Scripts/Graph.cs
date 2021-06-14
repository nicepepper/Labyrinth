using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    public List<GraphVertex> Vertices { get; }

    public Graph()
    {
        Vertices = new List<GraphVertex>();
    }

    public void AddVertex(string vertexName)
    {
        Vertices.Add(new GraphVertex(vertexName));
    }

    public GraphVertex FindVertex(string vertexName)
    {
        foreach (var vertex in Vertices)
        {
            if (vertex.Name.Equals(vertexName))
            {
                return vertex;
            }
        }

        return null;
    }

    public void AddEdge(string firstName, string secondName, int weight)
    {
        var firstVertex = FindVertex(firstName);
        var secondVertex = FindVertex(secondName);
        if (secondVertex != null && firstVertex != null)
        {
            firstVertex.AddEdge(secondVertex, weight);
            secondVertex.AddEdge(firstVertex, weight);
        }
    }
}
