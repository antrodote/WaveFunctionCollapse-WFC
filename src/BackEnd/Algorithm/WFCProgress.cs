using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase abstracta que representa un progreso en el algoritmo WFC.
/// </summary>
public abstract class WFCProgress { }

///<summary>
///  Clase abstracta que representa un progreso visualizable en el algoritmo WFC.
/// </summary>
public abstract class WFCVisualizableProgress : WFCProgress {

    public MapCell mapCell;
    public Module module;

}

///<summary>
///  Clase que representa la eliminación de un modulo en una celda en la ejecución del algoritmo WFC.
/// </summary>
public class WFCMapCellModuleRemoved : WFCVisualizableProgress
{
    public WFCMapCellModuleRemoved(MapCell mapCell, Module module)
    {

        this.mapCell = mapCell;
        this.module = module;

    }

}


///<summary>
///  Clase que representa el colapso de una celda a un modulo en el algoritmo WFC.
/// </summary>
public class WFCMapCellCollapsed : WFCVisualizableProgress
{
    public WFCMapCellCollapsed(MapCell mapCell, Module module)
    {
        this.mapCell = mapCell;
        this.module = module;
    }

}



///<summary>
///  Clase que representa la contradicción en una celda en la ejecución del algoritmo WFC.
/// </summary>
public class WFCContradiction : WFCVisualizableProgress
{

    public MapCell source;

    public WFCContradiction(MapCell mapCell, Module module, MapCell source)
    {
        this.mapCell = mapCell;
        this.module = null;
        this.source = source;
    }

}


///<summary>
///  Clase que representa el reinicio de la función de onda en la ejecución del algoritmo WFC.
/// </summary>
public class WFCReset : WFCProgress { }

///<summary>
///  Clase que representa el colapso de la función de onda en la ejecución del algoritmo WFC.
/// </summary>
public class WFCCollapsed : WFCProgress { }

///<summary>
///  Clase que representa una correcta propagación en la ejecución del algoritmo WFC.
/// </summary>
public class WFCPropagationSuccessful : WFCProgress { }

///<summary>
///  Clase que representa un error en la ejecución del algoritmo WFC.
/// </summary>
public class WFCError : WFCProgress {

    public string errorMessage;

    public WFCError(string errorMessage) {

        this.errorMessage = errorMessage;
    }

}

///<summary>
///  Clase que representa que en el estado actual del algoritmo WFC es imposible que la función de onda colapse.
/// </summary>
public class WFCUncollapsible : WFCError
{

    public WFCUncollapsible(string errorMessage) : base(errorMessage)
    {
        
    }

}



