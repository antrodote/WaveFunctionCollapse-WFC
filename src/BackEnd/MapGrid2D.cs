using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase que representa una cuadricula en un entorno de dos dimensiones.
/// </summary>
public class MapGrid2D : MapGrid
{

    ///<summary>
    ///  Anchura de la cuadricula.
    /// </summary>
    public int width;

    ///<summary>
    ///  Altura de la cuadricula.
    /// </summary>
    public int height;

    ///<summary>
    ///  Constructor para una cuadricula en un entorno de dos dimensiones.
    /// </summary>
    /// <param name="width">Anchura de la cuadricula.</param>
    /// <param name="height">Altura de la cuadricula.</param>
    /// <param name="optionsPerMapCell">Opciones disponibles para cada celda de la cuadricula.</param>
    public MapGrid2D(int width,int height,List<Module> optionsPerMapCell) {

        this.width = width;
        this.height = height;
        this.mapCells = new List<MapCell>();
        CreateMapGrid2D(optionsPerMapCell);
    
    }

    ///<summary>
    ///  Metodo auxiliar para general el grid con las celdas correspondientes.
    /// </summary>
    protected virtual void CreateMapGrid2D(List<Module> optionsPerMapCell) {

        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.height; y++) {

                Vector3Int mapCellCoords = new Vector3Int(x, y, 0);
                MapCell2D mapCell2D = new MapCell2D(mapCellCoords, optionsPerMapCell,true);
                mapCells.Add(mapCell2D);

            }
        }

    }

    public override Vector3Int GetDimensions()
    {
        return new Vector3Int(this.width, this.height, 0);
    }

    public override string ToString()
    {
        Vector3Int mapGridDimensions = this.GetDimensions();
        string res = "Map Grid x: " + mapGridDimensions.x + " " + "Map Grid y: " + mapGridDimensions.y + " " + "Map Grid z: " + mapGridDimensions.z + "\n";
        res += "Cells: \n";
        List<MapCell> mapGridCells = this.mapCells;
        foreach (MapCell mapCell in mapGridCells)
        {
            res += mapCell.ToString();
            res += "\n";
        }

        return res;

        
    }




}
