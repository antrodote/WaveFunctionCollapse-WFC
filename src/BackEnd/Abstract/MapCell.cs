using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/// <summary>
///  Clase abstracta que representa una celda de la cuadricula a la que queremos aplicar el algoritmo WFC.
/// </summary>
public abstract class MapCell
{

    /// <summary>
    ///  Coordenadas de la celda.
    /// </summary>
    protected Vector3Int coords;

    /// <summary>
    ///  Lista de todas las opciones disponibles para la celda.
    /// </summary>
    protected List<Module> options;

    /// <summary>
    ///  Lista de opciones invalidas para la celda.
    /// </summary>
    protected List<Module> invalidOptions;

    /// <summary>
    ///  ¿Es renderizable por pantalla la celda?
    /// </summary>
    protected bool renderizable;

    ///<summary>
    ///  Este metodo nos da información sobre todas las opciones disponibles para la celda.
    /// </summary>
    /// <returns>Una lista de todas las opciones disponibles para la celda</returns>
    public List<Module> GetOptions()
    {
        return this.options;
    }

    ///<summary>
    ///  Este método nos da información sobre las opciones invalidas para la celda.
    /// </summary>
    /// <returns>Una lista de todas las opciones invalidas para la celda</returns>
    public List<Module> GetInvalidOptions()
    {
        return this.invalidOptions;
    }

    ///<summary>
    ///  Este método nos da información sobre las opciones a las que puede colapsar la celda.
    /// </summary>
    /// <returns>Una lista de todas las opciones a las que puede colapsar la celda</returns>
    public List<Module> GetValidOptions()
    {

        if (options == null || invalidOptions == null)
        {
            Debug.LogError("MapCell_GetValidOptions: options property or invalidOptions property mustn't be null");
            return null;
        }

        List<Module> validOptions = options.Except(invalidOptions).ToList();
        return validOptions;

    }
    
    ///<summary>
    ///  Este método nos da información sobre la entriopia de la celda.
    /// </summary>
    /// <returns>La entriopia de la celda</returns>
    public int GetEntropy() {

        return this.GetValidOptions() == null ? -1 : this.GetValidOptions().Count;
    }

    ///<summary>
    ///  Este método nos da información sobre si la celda se encuentra colapsado o no.
    /// </summary>
    /// <returns>Un booleano indicando si la celda a colapsado o no</returns>
    public bool IsCollapsed()
    {
        return this.GetValidOptions().Count == 1;
    }

    ///<summary>
    ///  Este método nos da información sobre si la celda cuenta con una contradicción o no.
    /// </summary>
    /// <returns>Un booleano indicando si la celda cuenta con una contradicción o no</returns>
    public bool IsContradicted()
    {
        return this.GetValidOptions().Count == 0;
    }

    ///<summary>
    ///  Este método nos da información sobre si la celda debe ser renderizada o no por pantalla.
    /// </summary>
    /// <returns>Un booleano indicando si la celda debe ser renderizada o no</returns>
    public bool IsRenderizable()
    {
        return this.renderizable;
    }

    ///<summary>
    ///  Este método nos da información sobre las coordenadas celda en una determinada cuadricula.
    /// </summary>
    /// <returns>Un vector que representa la posición de la celda en la cuadricula</returns>
    public Vector3Int GetCoords()
    {
        return this.coords;
    }

    ///<summary>
    ///  Este método nos da información sobre los vecinos existente de la celda.
    /// </summary>
    /// <param name="mapGrid">La cuadricula a la que pertenece la celda</param>
    /// <returns>Una lista con las posiciones en la cuadricula de las celdas vecinas de la celda</returns>
    public abstract List<Vector3Int> GetMapCellNeighborsCoords<TMapGrid>(TMapGrid mapGrid) where TMapGrid : MapGrid;


    public override bool Equals(object obj)
    {
        if (obj is MapCell other)
        {
            return other.coords == this.coords;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return this.coords.GetHashCode();
    }

    public override string ToString()
    {
        string res = "Cell:\n ";
        res += "Coords: " + this.GetCoords().ToString() + "\n";
        res += "Options:\n";
        foreach (Module option in this.GetOptions()) {

            res += "Option: " + option.ToString();
            res += "\n";
        }

        res += "Invalid options:\n";
        foreach (Module option in this.GetInvalidOptions())
        {

            res += "Invalid Option: " + option.ToString();
            res += "\n";
        }

        res += "Valid options:\n";
        foreach (Module option in this.GetValidOptions())
        {

            res += "Valid Option: " + option.ToString();
            res += "\n";
        }


        return res;
    }


}
