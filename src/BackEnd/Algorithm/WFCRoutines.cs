using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


///<summary>
///  Clase que implementa el algoritmo WFC a través de las distintas operaciones realizadas en el.
/// </summary>
public class WFCRoutines<TMapGrid> : WFCRunner where TMapGrid : MapGrid
{

    ///<summary>
    ///  Función de onda.
    /// </summary>
    public TMapGrid wfc;


    ///<summary>
    ///  Constructor de algoritmo WFC sobre una función de onda.
    /// </summary>
    /// <param name="wfc">La función de onda a colapsar</param>
    public WFCRoutines(TMapGrid wfc)
    {
        this.wfc = wfc;
    }

    public override IEnumerable<WFCProgress> Collapse(bool collapseRequired)
    {
        
        while (!this.wfc.mapCells.AreMapCellsCollapsed())
        {
            foreach (WFCProgress progress in CollapseMinEntropyMapCell()) {
                
                yield return progress;
              
                switch (progress) {

                    case WFCUncollapsible wfcUncollapsible:
                        yield break;

                    case WFCContradiction contradiction:

                        if (!collapseRequired) yield break;

                            foreach (WFCProgress anotherProgress in Reset()) {
                            yield return anotherProgress;
                        }
                        break;
                        
                    case WFCMapCellCollapsed mapCellCollapsed: break;
                    case WFCMapCellModuleRemoved mapCellModuleRemoved: break;
                    default: break;
                
                }         
            }
        }

        yield return new WFCCollapsed();

    }

    public override IEnumerable<WFCProgress> CollapseMinEntropyMapCell()
    {

        MapCell minEntropyCell = this.wfc.mapCells.GetMinEntropyCell();
        if (minEntropyCell == null)
        {
            yield return new WFCUncollapsible("Imposible encontrar solucion para el WFC actual (Aplicar reset)");
            yield break;
        }
        else {
           
            foreach (WFCProgress progress in this.CollapseMapCell(minEntropyCell)) {
                yield return progress;
            }
        
        }
            
        
    }

    public override IEnumerable<WFCProgress> CollapseMapCell(MapCell mapCell)
    {
        
        List<Module> mapCellValidOptions = mapCell.GetValidOptions();
        int moduleToCollapseIndex = Random.Range(0, mapCellValidOptions.Count);
        Module moduleToCollapse = mapCellValidOptions[moduleToCollapseIndex];
        return this.CollapseMapCellToModule(mapCell, moduleToCollapse);

    }

    public override IEnumerable<WFCProgress> CollapseMapCellToModule(MapCell mapCell, Module module)
    {
        List<Module> mapCellOptions = mapCell.GetOptions();
        List<Module> mapCellInvalidOptions = mapCell.GetInvalidOptions();
        List<Module> newInvalidOptions = mapCellOptions.Where(moduleOption => !mapCellInvalidOptions.Contains(moduleOption))
            .Where(moduleOption => !moduleOption.Equals(module)).ToList();

        mapCellInvalidOptions.AddRange(newInvalidOptions);
        foreach (Module removedModule in newInvalidOptions) {
            yield return new WFCMapCellModuleRemoved(mapCell, removedModule);
        }

        yield return new WFCMapCellCollapsed(mapCell,module);
        foreach (WFCProgress progress in this.Propagate(mapCell)) {
            yield return progress;
        }

    }

    public override IEnumerable<WFCProgress> Propagate(MapCell source)
    {

        Stack<MapCell> mapCellStack = new Stack<MapCell>();
        //HashSet<MapCell> mapCellsVisited = new HashSet<MapCell>();
        //mapCellsVisited.Add(source);
        mapCellStack.Push(source);

        while (mapCellStack.Count > 0)
        {

            MapCell currentMapCell = mapCellStack.Pop();
            foreach (Vector3Int neighborMapCellCoords in currentMapCell.GetMapCellNeighborsCoords(this.wfc))
            {

                MapCell neighborMapCell = this.wfc.mapCells.GetCellByCoords(neighborMapCellCoords);
                //if (mapCellsVisited.Contains(neighborMapCell)) continue;

                List<Module> currentMapCellValidOptions = currentMapCell.GetValidOptions();
                List<Module> neighborMapCellValidOptions = neighborMapCell.GetValidOptions();
              
                foreach (Module neighborMapCellValidOption in neighborMapCellValidOptions)
                {
                    bool toRemove = !currentMapCellValidOptions.Any(currentMapCellValidOption => 
                    currentMapCellValidOption.constraint.ShouldRemoveModule(neighborMapCellValidOption.constraint,
                    currentMapCell.GetCoords(), 
                    neighborMapCellCoords) == false);

                    if (toRemove)
                    {
                        foreach (WFCProgress progress in RemoveMapCellOption(neighborMapCell, neighborMapCellValidOption)) {
                            yield return progress;
                        }
                        mapCellStack.Push(neighborMapCell);
                    }
                }//Fin foreach

                if (neighborMapCell.IsContradicted())
                {
                    yield return new WFCContradiction(neighborMapCell,null, currentMapCell);
                    yield break; 
                }
                if (neighborMapCell.IsCollapsed())
                {
                    yield return new WFCMapCellCollapsed(neighborMapCell,neighborMapCell.GetValidOptions()[0]);
                }

                //mapCellsVisited.Add(neighborMapCell);

            }//Fin foreach

        }//Fin while

        
    }//Fin function


    public override IEnumerable<WFCProgress> RemoveMapCellOption(MapCell mapCell, Module module)
    {

        mapCell.GetInvalidOptions().Add(module);
        yield return new WFCMapCellModuleRemoved(mapCell, module);
    }

    public override IEnumerable<WFCProgress> RemoveMapCellOptionFromUser(MapCell mapCell, Module module)
    {

        mapCell.GetInvalidOptions().Add(module);
        yield return new WFCMapCellModuleRemoved(mapCell, module);
        foreach (WFCProgress progress in this.Propagate(mapCell)){
            yield return progress;
        }
    }


    public override IEnumerable<WFCProgress> Reset()
    {

        foreach (MapCell mapCell in this.wfc.mapCells)
        {
            mapCell.GetInvalidOptions().Clear();
        }
        yield return new WFCReset();

    }

    public override MapGrid GetMapGrid()
    {
        return this.wfc;
    }


    public override string ToString()
    {
        return "WFC:\n" + this.wfc.ToString();

    }


}
