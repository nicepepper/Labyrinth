using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TestDijkstra : MonoBehaviour
{
   [SerializeField] private MapSettings _mapSettings;
   private Graph _graph =  new Graph();

   private void Awake()
   {
      foreach (var vertex in _mapSettings.vertices)
      {
         _graph.AddVertex(vertex.Name);
      }

      foreach (var adjacency in _mapSettings.adjacencies)
      {
         _graph.AddEdge(adjacency.FirstName, adjacency.SecondName, adjacency.EdgeWeight);
      }
        
      var dijkstra = new Dijkstra(_graph);
      var path = dijkstra.GetNumberVerticesInPath("9", "9");
      Debug.Log(path);
   }
}
