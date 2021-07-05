using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Dijkstra
{
    private Graph _graph;
    private List<GraphVertexInfo> _infos;
    
    public Dijkstra(Graph graph)
    {
        this._graph = graph;
    }

    public int GetNumberVerticesInPath(string startVertex, string finishVertex)
    {
        return GetNumberVerticesInPath(_graph.FindVertex(startVertex), _graph.FindVertex(finishVertex));
    }

    public int GetNumberVerticesInPath(GraphVertex startVertex, GraphVertex finishVertex)
    {
        int numberVertices = 1;
        FindShortesPath(startVertex, finishVertex);
        while (startVertex != finishVertex)
        {
            finishVertex = GetVertexInfo(finishVertex).PreviousVertex;
            numberVertices++;
        }
        
        return numberVertices;
    }

    // public string GetPath(string startVertex, string finishVertex)
    // {
    //    return GetPath(_graph.FindVertex(startVertex), _graph.FindVertex(finishVertex));
    // }
    //
    // public string GetPath(GraphVertex startVertex, GraphVertex finishVertex)
    // {
    //     var path = finishVertex.ToString();
    //     FindShortesPath(startVertex, finishVertex);
    //     
    //     while (startVertex != finishVertex)
    //     {
    //         finishVertex = GetVertexInfo(finishVertex).PreviousVertex;
    //         path = finishVertex.ToString() + path;
    //     }
    //
    //     return path;
    // }

    private void InitInfo()
    {
        _infos = new List<GraphVertexInfo>();

        foreach (var vertex in _graph.Vertices)
        {
            _infos.Add(new GraphVertexInfo(vertex));
        }
    }

    private GraphVertexInfo GetVertexInfo(GraphVertex vertex)
    {
        foreach (var info in _infos)
        {
            if (info.Vertex.Equals(vertex))
            {
                return info;
            }
        }

        return null;;
    }

    private GraphVertexInfo FindUnvisitedVertexWithMinSum()
    {
        var minValue = int.MaxValue;
        GraphVertexInfo minVertexInfo = null;

        foreach (var info in _infos)
        {
            if (info.IsUnvisited && info.EdgesWeightSum < minValue)
            {
                minVertexInfo = info;
                minValue = info.EdgesWeightSum;
            }
        }

        return minVertexInfo;
    }
    
    private void SetSumToNextVertex(GraphVertexInfo info)
    {
        info.IsUnvisited = false;

        foreach (var edge in info.Vertex.Edges)
        {
            var nextInfo = GetVertexInfo(edge.ConnectedVertex);
            var sum = info.EdgesWeightSum + edge.EdgeWeight;

            if (sum < nextInfo.EdgesWeightSum)
            {
                nextInfo.EdgesWeightSum = sum;
                nextInfo.PreviousVertex = info.Vertex;
            }
        }
    }
    
    private void FindShortesPath(GraphVertex startVertex, GraphVertex finishVertex)
    {
        InitInfo();
        if (Equals(startVertex))
        {
            return;
        }
        var first = GetVertexInfo(startVertex);
        first.EdgesWeightSum = 0;

        while (true)
        {
            var current = FindUnvisitedVertexWithMinSum();
            
            if (current == null)
            {
                break;
            }
            
            SetSumToNextVertex(current);
        }
    }
}
