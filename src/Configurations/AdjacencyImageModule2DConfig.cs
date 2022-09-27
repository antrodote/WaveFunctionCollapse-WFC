using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///  Clase contenedera de una configuracion para modulos 2D con restricciones de adjacencia sobre imagenes.
/// </summary>
[CreateAssetMenu(fileName = "New Adjacency Image WFC 2D Config", menuName = "antrodote WFC/Adjacency Image WFC 2D Config")]
public class AdjacencyImageModule2DConfig : WFConfigObject<AdjacencyImageModule2D, AdjacencyImageConstraint2D, MapGrid2D>
{
    ///<summary>
    ///  Vector para la configuracion de una cuadricula de dos dimensiones.
    /// </summary>
    public Vector2Int size = new Vector2Int(0, 0);

    ///<summary>
    ///  Restriccion de adjacencia sobre imagenes para los limites de la cuadricula.
    /// </summary>
    public AdjacencyImageConstraint2D wrapper = new AdjacencyImageConstraint2D(null, new List<Sprite>(), new List<Sprite>(), new List<Sprite>(), new List<Sprite>());


    public override WFCRoutines<MapGrid2D> CreateWFC()
    {
        if (this.size.x == 0 || this.size.y == 0)
        {
            Debug.Log("Invalid size for the grid");
            return null;
        }

        for (int i = 0; i < modules.Count; i++)
        {
            modules[i].constraint = constraints[i];  
        }

        
        List<Module> optionsPerCell = new List<Module>();
        optionsPerCell.AddRange(modules);
        MapGrid2D mapGrid = new MapGrid2D(this.size.x, this.size.y, optionsPerCell);
        WFCRoutines<MapGrid2D> wfcSolver = new WFCRoutines<MapGrid2D>(mapGrid);
        CreateWrapper(wfcSolver);
        return wfcSolver;

    }

    ///<summary>
    ///  Metodo auxiliar para generar un wrapper sobre la cuadricula a la que queremos aplicar el algoritmo WFC.
    /// </summary>
    /// <param name="wfcSolver">Funcion de onda a la que establecer el wrapper</param>
    private void CreateWrapper(WFCRunner wfcSolver) {


        if (wrapper.sprite == null) return;
        AdjacencyImageModule2D wrapperModule= new AdjacencyImageModule2D(wrapper.sprite, wrapper);
        List<Module> module = new List<Module>();
        module.Add(wrapperModule);
        List<MapCell> wrapperMapCells = new List<MapCell>();
        
        for (var x = 0; x < this.size.x; x++) {
            Vector3Int coords1 = new Vector3Int(x, -1 , 0);
            Vector3Int coords2 = new Vector3Int(x, this.size.y , 0);
            wrapperMapCells.Add(new MapCell2D(coords1, module,false));
            wrapperMapCells.Add(new MapCell2D(coords2, module,false));
        }

        for (var y = 0; y < this.size.y; y++) {
            Vector3Int coords1 = new Vector3Int(-1, y , 0);
            Vector3Int coords2 = new Vector3Int(this.size.x , y , 0);
            wrapperMapCells.Add(new MapCell2D(coords1, module,false));
            wrapperMapCells.Add(new MapCell2D(coords2, module,false));
        }

        wfcSolver.GetMapGrid().mapCells.AddRange(wrapperMapCells);
        foreach (MapCell wrapperMapCell in wrapperMapCells) {

            foreach (var progress in wfcSolver.CollapseMapCellToModule(wrapperMapCell, wrapperModule)) {

                switch (progress) {

                    case WFCMapCellModuleRemoved wfcMapCellModuleRemoved:
                        break;

                    case WFCMapCellCollapsed wfcMapCellCollapsed:
                        break;

                    case WFCContradiction wfcContradiction:
                        break;

                       
                }

            }

            
        }


    }

    ///<summary>
    ///  Metodo que intenta obtener un sprite de un modulo.
    /// </summary>
    /// <param name="module">Modulo al cual se le intenta extraer sprite</param>
    /// <param name="sprite">Variable de salida donde se referenciara el sprite</param>
    public bool TryGetSprite(Module module, out Sprite sprite)
    {


        sprite = null;
        AdjacencyImageModule2D adjacencyModule2D = module as AdjacencyImageModule2D;
        if (adjacencyModule2D == null) return false;
        sprite = adjacencyModule2D.sprite;
        return true;

    }


}
