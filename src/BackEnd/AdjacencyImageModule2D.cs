using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase que representa un modulo 2D con restricción de adjacencia con imagenes.
/// </summary>

[System.Serializable]
public class AdjacencyImageModule2D : Module2D
{
    ///<summary>
    ///  Constructor de modulo 2D con restricción de adjacencia con imagenes.
    /// </summary>
    public AdjacencyImageModule2D(Sprite sprite, AdjacencyImageConstraint2D adjacencyImageConstraint2D)
    {
        this.sprite = sprite;
        this.display = sprite != null ? sprite.name : null;
        this.constraint = adjacencyImageConstraint2D;
    }
}
