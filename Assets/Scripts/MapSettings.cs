using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu (fileName = "GameSettings", menuName = "Labyrinth/GameSettings", order = 2)]
public class MapSettings : ScriptableObject
{
    [SerializeField] public List<Vertex> vertices = new List<Vertex>()
    {
        new Vertex("1", new Point(0.25f, 18.0f), new Point(12.0f, 23.75f)),
        new Vertex("2", new Point(12.0f, 12.0f), new Point(22.0f, 23.75f)),
        new Vertex("3", new Point(22.0f, 12.0f), new Point(32.0f, 23.75f)),
        new Vertex("4", new Point(32.0f, 18.0f), new Point(43.75f, 23.75f)),
        new Vertex("5", new Point(0.25f, 6.0f), new Point(6.0f, 18.0f)),
        new Vertex("6", new Point(6.0f, 6.0f), new Point(12.0f, 18.0f)),
        new Vertex("7", new Point(32.0f, 6.0f), new Point(37.75f, 18.0f)),
        new Vertex("8", new Point(37.75f, 6.0f), new Point(43.75f, 18.0f)),
        new Vertex("9", new Point(0.25f, 0.25f), new Point(12.0f, 6.0f)),
        new Vertex("10", new Point(12.0f, 0.25f), new Point(22.0f, 12.0f)),
        new Vertex("11", new Point(22.0f, 0.25f), new Point(32.0f, 12.0f)),
        new Vertex("12", new Point(32.0f, 0.25f), new Point(43.75f, 6.0f))
    };

    [SerializeField] public List<Adjacencies> adjacencies = new List<Adjacencies>()
    {
        new Adjacencies("1","2",1),
        new Adjacencies("1","5",1),
        new Adjacencies("1","6",1),
        new Adjacencies("2","3",1),
        new Adjacencies("2","10",1),
        new Adjacencies("3","4",1),
        new Adjacencies("3","11",1),
        new Adjacencies("4","7",1),
        new Adjacencies("4","8",1),
        new Adjacencies("5","6",1),
        new Adjacencies("5","9",1),
        new Adjacencies("6","9",1),
        new Adjacencies("7","8",1),
        new Adjacencies("7","12",1),
        new Adjacencies("8","12",1),
        new Adjacencies("9","10",1),
        new Adjacencies("10","11",1),
        new Adjacencies("11","12",1)
    };
}

[Serializable]
public class Adjacencies
{
    [SerializeField] private string _firstName;
    [SerializeField] private string _secondName;
    [Tooltip("Cost of Path")]
    [SerializeField] private int _edgeWeight;

    public Adjacencies(string firstName, string secondName, int edgeWeight)
    {
        _firstName = firstName;
        _secondName = secondName;
        _edgeWeight = edgeWeight;
    }
    
    public string FirstName => _firstName;

    public string SecondName => _secondName;

    public int EdgeWeight => _edgeWeight;
}

[Serializable]
public class Vertex
{
    [Tooltip("Name vertex")]
    [SerializeField] private string _name;
    //[Tooltip("We look at the map in the global planes [x, z]")]
    [Header("Coordinates of the Rectangle")]
    [Tooltip("Bottom left corner. We look at the map in the global planes [x, z]")] 
    [SerializeField] private Point _firstPoint;
    [Tooltip("Upper right corner. We look at the map in the global planes [x, z]")] 
    [SerializeField] private Point _secondPoint;
    
    public Vertex(string name, Point firstPoint, Point secondPoint)
    {
        _name = name;
        _firstPoint = firstPoint;
        _secondPoint = secondPoint;
    }
    
    public string Name => _name;

    public Point FirstPoint => _firstPoint;

    public Point SecondPoint => _secondPoint;
}

[Serializable]
public class Point
{
    [SerializeField] private float _x;
    [SerializeField] private float _z;

    public Point(float x, float z)
    {
        _x = x;
        _z = z;
    }

    public float X { get; set; }

    public float Z { get; set; }
}
