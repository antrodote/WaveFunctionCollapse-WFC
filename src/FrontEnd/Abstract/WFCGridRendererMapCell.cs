using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase abstracta encargada de gestionar la visualizacion de las celdas de una funcion de onda.
/// </summary>
public abstract class WFCGridRendererMapCell : MonoBehaviour
{

    ///<summary>
    ///  Renderizador de la funcion de onda.
    /// </summary>
    public WFCGridRenderer wfcGridRenderer;

    ///<summary>
    ///  Celda de la funcion de onda.
    /// </summary>
    public MapCell mapCell;

    ///<summary>
    ///  Prefab para la visualizacion y control de los modulos de la celda.
    /// </summary>
    public WFCGridRendererMapCellOption optionPrefab;

    ///<summary>
    ///  Prefab para la visualizacion de una contradiccion de la celda.
    /// </summary>
    public GameObject contradictionPrefab;

    ///<summary>
    ///  Diccionario de corrrespondencia entre un modulo de la celda y su objeto de renderizado en la escena.
    /// </summary>
    public Dictionary<Module, WFCGridRendererMapCellOption> moduleToRenderer = new Dictionary<Module, WFCGridRendererMapCellOption>();

    ///<summary>
    ///  Metodo que gestionara la renderizacion de la celda en el momento de su creacion.
    /// </summary>
    /// <param name="mapCell">Celda que se quiere renderizar</param>
    /// <param name="wfcGridRenderer">Renderizador de la funcion de onda</param>
    public abstract void OnCreated(MapCell mapCell,WFCGridRenderer wfcGridRenderer);

    ///<summary>
    ///  Metodo que manda a eliminar la renderizacion de un modulo perteneciente a la celda.
    /// </summary>
    /// <param name="removalModule">Modulo del que se quiere eliminar la renderizacion</param>
    public void RemoveOption(Module removalModule) {

        if (!moduleToRenderer.ContainsKey(removalModule)) return;
        moduleToRenderer[removalModule].Remove();
    
    }

    ///<summary>
    ///  Metodo que manda a mostrar el colapso de la celda a un modulo.
    /// </summary>
    /// <param name="selectModule">Modulo seleccionado</param>
    public void SelectOption(Module selectModule) {

        if (!moduleToRenderer.ContainsKey(selectModule)) return;
        moduleToRenderer[selectModule].Select();

    }

    ///<summary>
    ///  Metodo encargado mostrar que la celda sufre de una contradiccion.
    /// </summary>
    public abstract void ShowContradiction();

    ///<summary>
    ///  Metodo encargado de desactivar el marco correspondiente a la celda en el caso de que este exista.
    /// </summary>
    public abstract void DisableFrame();
    

}
