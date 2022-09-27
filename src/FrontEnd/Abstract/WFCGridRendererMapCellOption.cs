using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///  Clase abstracta encargada de gestionar la visualizacion de los modulos de una funcion de onda.
/// </summary>
public abstract class WFCGridRendererMapCellOption : MonoBehaviour 
{
    ///<summary>
    ///  Renderizador de la celda a la que pertenece el modulo.
    /// </summary>
    public WFCGridRendererMapCell mapCellRenderer;

    ///<summary>
    ///  Posicion inicial en el espacio.
    /// </summary>
    public Vector3 startPosition;

    ///<summary>
    ///  Escala inicial en el espacio.
    /// </summary>
    public Vector3 startScale;

    ///<summary>
    ///  Posicion del modulo al colapsar una celda a el.
    /// </summary>
    public Vector3 selectedPosition;

    ///<summary>
    ///  Escala del modulo al colapsar una celda a el.
    /// </summary>
    public Vector3 selectionScale;

    ///<summary>
    ///  Indica si el modulo esta disponible para interaccionar con el.
    /// </summary>
    public bool available = true;

    ///<summary>
    ///  Indica si la celda a la que pertenece a colapsado a el.
    /// </summary>
    public bool isOnly = false;

    ///<summary>
    ///  Metodo que gestionara la renderizacion del modulo en el momento de su creacion.
    /// </summary>
    /// <param name="module">Modulo que se quiere renderizar</param>
    /// <param name="mapCellRenderer">Renderizador de la celda a la que pertenece</param>
    public abstract void OnCreated(Module module,WFCGridRendererMapCell mapCellRenderer);

    ///<summary>
    ///  Metodo para indicar que el modulo debe ser removido.
    /// </summary>
    public void Remove() {
        available = false;
    }

    ///<summary>
    ///  Metodo para indicar que el modulo ha sido seleccionado como colapso de una celda.
    /// </summary>
    public void Select() {
        isOnly = true;
    }


}
