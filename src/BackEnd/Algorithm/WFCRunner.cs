using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase abstracta que define las operaciones del algoritmo WFC.
/// </summary>
public abstract class WFCRunner
{
   
    ///<summary>
    ///  Metodo que intenta colapsar la función de onda.
    /// </summary>
    /// <param name="collapsedRequired">¿Se requiere un colapso de la funcion de onda obligatoriamente?</param>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> Collapse(bool collapsedRequired);
    
    ///<summary>
    ///  Metodo que intenta colapsar la celda de la cuadricula con la menor entriopia.
    /// </summary>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> CollapseMinEntropyMapCell();

    ///<summary>
    ///  Metodo que intenta colapsar una celda de la cuadricula.
    /// </summary>
    /// <param name="mapCell">Celda que queremos intentar colapsar</param>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> CollapseMapCell(MapCell mapCell);

    ///<summary>
    ///  Metodo que intenta colapsar una celda a un modulo.
    /// </summary>
    /// <param name="mapCell">Celda que queremos intentar colapsar a un modulo</param>
    /// <param name="module">Modulo al que queremos colapsar la celda</param>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> CollapseMapCellToModule(MapCell mapCell, Module module);

    ///<summary>
    ///  Metodo que intenta propagar informacion entre las celdas de la cuadricula.
    /// </summary>
    /// <param name="source">Celda fuente sobre la que inicia la propagacion</param>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> Propagate(MapCell source);
    
    ///<summary>
    ///  Metodo que intenta eliminar una opcion disponible de colapso en una celda.
    /// </summary>
    /// <param name="mapCell">Celda a la que queremos eliminar una opcion disponible de colapso</param>
    /// <param name="module">Modulo que queremos eliminar como opcion disponible de colapso</param>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> RemoveMapCellOption(MapCell mapCell, Module module);

    ///<summary>
    ///  Metodo que intenta eliminar una opcion disponible de colapso en una celda siendo este elegido por el usuario.
    /// </summary>
    /// <param name="mapCell">Celda a la que queremos eliminar una opcion disponible de colapso</param>
    /// <param name="module">Modulo que queremos eliminar como opcion disponible de colapso</param>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> RemoveMapCellOptionFromUser(MapCell mapCell, Module module);

    ///<summary>
    ///  Metodo que intenta establecer la funcion de onda a su estado inicial.
    /// </summary>
    /// <returns>Un WFCProgress que permite observar la ultima operacion realizada por el algoritmo WFC</returns>
    public abstract IEnumerable<WFCProgress> Reset();
    
    ///<summary>
    ///  Metodo que nos da informacion sobre la funcion de onda con la que esta trabajando el algoritmo WFC.
    /// </summary>
    /// <returns>La funcion de onda con la que esta trabajando el algoritmo WFC</returns>
    public abstract MapGrid GetMapGrid();

}
