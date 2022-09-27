using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase que representa una celda en un entorno de dos dimensiones.
/// </summary>
public class MapCell2D : MapCell
{

    ///<summary>
    ///  Constructor para una celda en un entorno de dos dimensiones.
    /// </summary>
    /// <param name="coords">Coordenadas de la celda</param>
    /// <param name="options">Lista de todos los posibles opciones para la celda.</param>
    /// <param name="renderizable">¿Es renderizable la celda?</param>
    public MapCell2D(Vector3Int coords, List<Module> options,bool renderizable) {

        this.coords = coords;
        this.options = options;
        this.invalidOptions = new List<Module>();
        this.renderizable = renderizable;
    }

    public override List<Vector3Int> GetMapCellNeighborsCoords<TMapGrid>(TMapGrid mapGrid)
    {


        bool hasLeftNeighbor;
        bool hasRightNeighbor;
        bool hasTopNeighbor;
        bool hasBottomNeighbor;

        List<Vector3Int> neighborsCoords = new List<Vector3Int>();

        MapGrid2D  mapGrid2D = mapGrid as MapGrid2D;
        if (mapGrid2D == null) return null;

        Vector3Int currentPosition = this.GetCoords();

        
        hasLeftNeighbor = currentPosition.x - 1 >= 0 && currentPosition.y >= 0 && currentPosition.y < mapGrid2D.height ? true : false ;
        hasRightNeighbor = currentPosition.x + 1 < mapGrid2D.width && currentPosition.y >=0 && currentPosition.y < mapGrid2D.height  ? true : false;
        hasTopNeighbor = currentPosition.y + 1 < mapGrid2D.height && currentPosition.x >= 0 && currentPosition.x < mapGrid2D.width ? true : false;
        hasBottomNeighbor = currentPosition.y - 1 >= 0 && currentPosition.x >= 0 && currentPosition.x < mapGrid2D.width ? true : false;


        if (hasLeftNeighbor) neighborsCoords.Add(new Vector3Int(currentPosition.x - 1 , currentPosition.y , currentPosition.z));
        if (hasRightNeighbor) neighborsCoords.Add(new Vector3Int(currentPosition.x + 1 , currentPosition.y , currentPosition.z));
        if (hasTopNeighbor) neighborsCoords.Add(new Vector3Int(currentPosition.x , currentPosition.y + 1 , currentPosition.z));
        if (hasBottomNeighbor) neighborsCoords.Add(new Vector3Int(currentPosition.x , currentPosition.y - 1 , currentPosition.z));


        return neighborsCoords;
        

    }

}
