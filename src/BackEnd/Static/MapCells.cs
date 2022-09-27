using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
///  Clase estatica para el manejo de listas de MapCell mediante programación funcional y metodos de extensión.
/// </summary>
public static class MapCells 
{
    ///<summary>
    ///  Dada una lista de MapCell nos da información sobre la celda con la menor entriopia.
    /// </summary>
    /// <param name="mapCells">Lista de MapCell</param>
    /// <returns>La celda con la menor entriopia en la lista</returns>
    public static MapCell GetMinEntropyCell(this List<MapCell> mapCells) {

        return mapCells.Where(mapCell => mapCell.GetEntropy() > 1).OrderBy(mapCell => mapCell.GetValidOptions().Count).FirstOrDefault();
    
    }

    ///<summary>
    ///  Dada una lista de MapCell y unas coordenadas se intenta encontrar un MapCell ubicado en esas coordenadas.
    /// </summary>
    /// <param name="mapCells">Lista de MapCell</param>
    /// <param name="coords">Coordenas de la celda que queremos encontrar</param>
    /// <returns>La celda ubicada en las coordenadas especificadas</returns>
    public static MapCell GetCellByCoords(this List<MapCell> mapCells, Vector3Int coords) {

        return mapCells.Where(mapCell => mapCell.GetCoords().Equals(coords)).FirstOrDefault();    
    }

    public static bool AreMapCellsCollapsed(this List<MapCell> mapCells) {
        
        return mapCells.All(mapCell => mapCell.IsCollapsed());
    
    }

    ///<summary>
    ///  Dada una lista de MapCell nos da información sobre si alguna celda tiene una contradicción.
    /// </summary>
    /// <param name="mapCells">Lista de MapCell</param>
    /// <returns>Nos devuelve un booleano indicando si alguna de las celdas de la lista tiene alguna contradicción</returns>
    public static bool SomeContradiction(this List<MapCell> mapCells) {
            
        return mapCells.Any(mapCell => mapCell.IsContradicted());

    }

    ///<summary>
    ///  Dada una lista de MapCell nos da información sobre las celdas que cuentan con una contradicción.
    /// </summary>
    /// <param name="mapCells">Lista de MapCell</param>
    /// <returns>Nos devuelve una lista con todas las celdas que tienen una contradicción</returns>
    public static List<MapCell> ContradictedMapCells(this List<MapCell> mapCells)
    {

        return mapCells.Where(mapCell => mapCell.IsContradicted()).ToList();

    }

    ///<summary>
    ///  Dada una lista de MapCell nos da información sobre las celdas que son renderizables por pantalla.
    /// </summary>
    /// <param name="mapCells">Lista de MapCell</param>
    /// <returns>Nos devuelve una lista con todas las celdas que son renderizables por pantalla</returns>
    public static List<MapCell> RenderizableMapCells(this List<MapCell> mapCells)
    {

        return mapCells.Where(mapCell => mapCell.IsRenderizable()).ToList();

    }


    ///<summary>
    ///  Dada una lista de MapCell nos da información sobre las celdas que no son renderizables por pantalla.
    /// </summary>
    /// <param name="mapCells">Lista de MapCell</param>
    /// <returns>Nos devuelve una lista con todas las celdas que no son renderizables por pantalla
    public static List<MapCell> UnRenderizableMapCells(this List<MapCell> mapCells)
    {

        return mapCells.Where(mapCell => !mapCell.IsRenderizable()).ToList();

    }

}
