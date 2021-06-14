using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra
{
    private Graph _graph;
    private List<GraphVertexInfo> _infos;

    public Dijkstra(Graph graph)
    {
        this._graph = graph;
    }

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

    public GraphVertexInfo FindUnvisitedVertexWithMinSum()
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

    public string FindShortesPath(string startName, string finishName)
    {
        return FindShortesPath(_graph.FindVertex(startName), _graph.FindVertex(finishName));
    }

    public string FindShortesPath(GraphVertex startVertex, GraphVertex finishVertex)
    {
        InitInfo();
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

        return GetPath(startVertex, finishVertex);
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
    
    private string GetPath(GraphVertex startVertex, GraphVertex endVertex)
    {
        var path = endVertex.ToString();

        while (startVertex != endVertex)
        {
            endVertex = GetVertexInfo(endVertex).PreviousVertex;
            path = endVertex.ToString() + path;
        }

        return path;
    }
}
