using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase para visualizar la funcion de onda en los diferentes pasos del algoritmo WFC.
/// </summary>
public class WFCGridRenderer : MonoBehaviour
{

    ///<summary>
    ///  Configuracion de la que obtener la funcion de onda a la que se aplicarar el algoritmo WFC.
    /// </summary>    
    public WFConfigObject config;

    ///<summary>
    ///  Funcion onda sobre la que aplicar el algoritmo WFC.
    /// </summary>    
    public WFCRunner wfc;

    ///<summary>
    ///  Funcion onda auxiliar.
    /// </summary>    
    public WFCRunner auxWFC = null;

    ///<summary>
    ///  Camara de la escena.
    /// </summary>    
    public Camera cam;

    ///<summary>
    ///  Objeto de la escena que contendra la visualizacion de funcion de onda.
    /// </summary>    
    public GameObject board;


    ///<summary>
    ///  Controlador del estado del juego para activar o desabilitar la interaccion con el algoritmo WFC.
    /// </summary>
    public GameStatusHandler gameStatusHandler;

    ///<summary>
    ///  Prefab para la visualizacion y control de las celdas de la cuadricula.
    /// </summary>
    public WFCGridRendererMapCell mapCellPrefab;

    ///<summary>
    ///  Diccionario de corrrespondencia entre una celda de la cuadricula y su objeto de renderizado en la escena.
    /// </summary>
    public Dictionary<MapCell, WFCGridRendererMapCell> mapCellToRenderer = new Dictionary<MapCell, WFCGridRendererMapCell>();

    ///<summary>
    ///  Variable para controlar si el algoritmo WFC se puede aplicar desde el principio.
    /// </summary>
    private bool canSolveFromScratch = true;


    // Start is called before the first frame update
    void Start()
    {

        InitWFC();

    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a colapsar la celda con menor entriopia de nuestra funcion de onda.
    /// </summary>
    public void RelaxOne() {
        this.canSolveFromScratch = false;
        this.wfc.CollapseMinEntropyMapCell().RunAsCoroutine(this).onProgress += HandleWFCProgress;
    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a colapsar nuestra funcion de onda desde el estado inicial.
    /// </summary>
    public void Solve() {
        if (!canSolveFromScratch) return;
        this.wfc.Collapse(true).RunAsCoroutine(this).onComplete += HandleCompletedWFC;
        this.canSolveFromScratch = false;
    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a colapsar nuestra funcion de onda desde cualquier estado.
    /// </summary>
    public void SolveFromUser()
    {
        this.canSolveFromScratch = false;
        this.auxWFC = this.wfc;
        this.wfc.Collapse(false).RunAsCoroutine(this).onProgress += HandleWFCProgress;
    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a eliminar un modulo de una celda de nuestra funcion de onda evitando que la celda colapse a el.
    /// </summary>
    public void RemoveModule(MapCell mapCell, Module option) {
        this.canSolveFromScratch = false;
        this.wfc.RemoveMapCellOptionFromUser(mapCell, option).RunAsCoroutine(this).onProgress += HandleWFCProgress;
    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a colapsar una celda a un modulo de nuestra funcion de onda.
    /// </summary>
    public void SelectionModule(MapCell mapCell, Module option) {
        this.canSolveFromScratch = false;
        this.wfc.CollapseMapCellToModule(mapCell, option).RunAsCoroutine(this).onProgress += HandleWFCProgress;
    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a volver al estado inicial de nuestra funcion de onda.
    /// </summary>
    public void Reset_WFC() {

        this.wfc.Reset().RunAsCoroutine(this).onProgress += HandleWFCProgress;

    }

    ///<summary>
    ///  Metodo que manda al algoritmo WFC a colapsar una celda a un modulo de nuestra funcion de onda.
    /// </summary>
    public void InitWFC()
    {

        this.wfc = config.CreateSpace();
        if (this.wfc == null) return;

        Debug.Log("Configuracion creada");
        SetCameraPosition(this.wfc.GetMapGrid().GetDimensions());
        this.InitGameObjects();

    }

    ///<summary>
    ///  Metodo que instacion todos los objetos necesarios en la escena para poder visualizar nuestra funcion de onda.
    /// </summary>
    public void InitGameObjects()
    {

        mapCellToRenderer.Clear();
        for (var i = 0; i < board.transform.childCount; i++)
        {
            Destroy(board.transform.GetChild(i).gameObject);

        }

        foreach (MapCell mapCell in this.wfc.GetMapGrid().mapCells.RenderizableMapCells())
        {

            WFCGridRendererMapCell mapCellInstance = Instantiate(mapCellPrefab, board.transform);
            mapCellInstance.transform.localScale = new Vector3(1, 1, 1);
            mapCellInstance.name = $"mapCell {mapCell.GetCoords()}";
            mapCellInstance.transform.localPosition = mapCell.GetCoords();
            mapCellToRenderer.Add(mapCell, mapCellInstance);
            mapCellInstance.OnCreated(mapCell,this);


        }


    }

    ///<summary>
    ///  Metodo para gestionar la finalizacion del algoritmo WFC y visualizar la funcion de onda en la escena.
    /// </summary>
    private void HandleCompletedWFC() {

        
        
        foreach (MapCell mapCell in wfc.GetMapGrid().mapCells.RenderizableMapCells())
        {

            foreach (Module invalidModule in mapCell.GetInvalidOptions()) {
                mapCellToRenderer[mapCell].RemoveOption(invalidModule);
            }

            mapCellToRenderer[mapCell].SelectOption(mapCell.GetValidOptions()[0]);



        }

    }

    ///<summary>
    ///  Metodo para gestionar el proceso del algoritmo WFC y visualizarlo en la escena.
    /// </summary>
    /// <param name="progress">Un progreso dentro del algoritmo WFC</param>
    private void HandleWFCProgress(WFCProgress progress) {

        switch (progress) {

            case WFCMapCellCollapsed wFCMapCellCollapsed:

                mapCellToRenderer[wFCMapCellCollapsed.mapCell].SelectOption(wFCMapCellCollapsed.module);
                break;

            case WFCMapCellModuleRemoved wfcMapCellModuleRemoved:

                mapCellToRenderer[wfcMapCellModuleRemoved.mapCell].RemoveOption(wfcMapCellModuleRemoved.module);
                break;

            case WFCUncollapsible wfcUncollapsible:

                List<MapCell> contradictedMapCells = this.wfc.GetMapGrid().mapCells.ContradictedMapCells();
                if (contradictedMapCells == null || contradictedMapCells.Count < 1) return;
                foreach (MapCell contradictedMapCell in contradictedMapCells) {

                    mapCellToRenderer[contradictedMapCell].ShowContradiction();

                }
                break;

            case WFCReset wfcReset:

                foreach (MapCell mapCell in this.wfc.GetMapGrid().mapCells.UnRenderizableMapCells())
                {
                    wfc.CollapseMapCell(mapCell).RunAsRoutine(null);
                }
                InitGameObjects();
                this.canSolveFromScratch = true;
                break;

            case WFCContradiction wfcContradiction:

                if (this.auxWFC == null)
                {
                    mapCellToRenderer[wfcContradiction.mapCell].ShowContradiction();

                }
                else {

                    this.wfc = this.auxWFC;
                    this.auxWFC = null;
                    this.SolveFromUser();

                }

                break;

        }
    
    }

    ///<summary>
    ///  Metodo que establece las dimension necesaria de la camara ortografica para visualizar la funcion de onda y la UI.
    /// </summary>
    private void SetCameraPosition(Vector3 gridDimensions) {

        if (this.cam == null) return;
        this.cam.transform.localPosition = new Vector3(gridDimensions.x / 2, gridDimensions.y / 2 , -10);
        //Sacado de buscar Understanding Orthographic Size in Unity | The correct orthographic size Unity
        //float screenHeightInUnits = gridDimensions.y / 2; //Desde el centro de la pantalla se visualizaran ese numero de unidades hacia arriba y hacia abajo
        //float screenWidthInUnits = gridDimensions.x * Screen.height / Screen.width * 0.5f; //Desde el centro de la pantalla se visualizaran ese numero de unidades hacia la derecha y hacia la izquierda
        float screenHeightInUnits = (gridDimensions.y + 1f) / 2; //Desde el centro de la pantalla se visualizaran ese numero de unidades hacia arriba y hacia abajo
        float screenWidthInUnits = (gridDimensions.x + 1f) * Screen.height / Screen.width * 0.5f; //Desde el centro de la pantalla se visualizaran ese numero de unidades hacia la derecha y hacia la izquierda
        this.cam.orthographicSize = screenHeightInUnits > screenWidthInUnits ? screenHeightInUnits : screenWidthInUnits;
    }



}
