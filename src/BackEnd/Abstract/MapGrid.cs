using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  Clase abstracta que representa la cuadricula sobre la que queremos aplicar el algoritmo WFC.
/// </summary>
public abstract class MapGrid
{

    /// <summary>
    ///  Lista de celdas de la cuadricula.
    /// </summary>
    public List<MapCell> mapCells;

    ///<summary>
    ///  Este método nos da información sobre las dimensiones de la cuadricula.
    /// </summary>
    /// <returns>Un vector de tres dimensiones que indican el ancho, alto y profundidad de la cuadricula</returns>
    public abstract Vector3Int GetDimensions();

}
