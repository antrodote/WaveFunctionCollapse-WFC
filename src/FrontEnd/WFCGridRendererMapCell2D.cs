using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
///  Clase  encargada de renderizar una celda de nuestra funcion de onda en un entorno 2D.
/// </summary>
public class WFCGridRendererMapCell2D : WFCGridRendererMapCell
{

    public override void DisableFrame()
    {
        if (this.transform.childCount < 1) return;
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public override void ShowContradiction()
    {
        for (var i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        moduleToRenderer.Clear();


        if (contradictionPrefab == null) return;
        GameObject mapCellOptionInstance = Instantiate(contradictionPrefab, this.transform);
        float instanceCoordX = 0.5f;
        float instanceCoordY = 0.5f;
        mapCellOptionInstance.name = $"option {mapCell.GetCoords()}_Contradiction";
        mapCellOptionInstance.transform.localScale = new Vector3(1, 1);
        mapCellOptionInstance.transform.localPosition = new Vector3(instanceCoordX, instanceCoordY, 0);




    }

    public override void OnCreated(MapCell mapCell,WFCGridRenderer wfcGridRenderer)
    {

        this.wfcGridRenderer = wfcGridRenderer;
        this.mapCell = mapCell;
        List<Module> mapCellValidOptions = this.mapCell.GetValidOptions();

        int cellDivisor = Mathf.CeilToInt(Mathf.Sqrt(mapCellValidOptions.Count));
        float subCellsRange = 1f / cellDivisor;
        moduleToRenderer.Clear();

        float coordX = 0.0f;
        float coordY = 0.0f;
        int placedOptions = 0;
        foreach (Module module in mapCellValidOptions)
        {

            WFCGridRendererMapCellOption mapCellOptionInstance = Instantiate(optionPrefab, this.transform);
            float instanceCoordX = coordX + subCellsRange / 2;
            float instanceCoordY = coordY + subCellsRange / 2; ;
            mapCellOptionInstance.name = $"option {mapCell.GetCoords()}_{module.display}";
            mapCellOptionInstance.transform.localScale = new Vector3(subCellsRange, subCellsRange);
            mapCellOptionInstance.transform.localPosition = new Vector3(instanceCoordX, instanceCoordY, 0);
            placedOptions++;
            if (placedOptions >= cellDivisor)
            {
                coordX = 0.0f;
                coordY = coordY + subCellsRange;
                placedOptions = 0;
            }
            else
            {

                coordX = coordX + subCellsRange;

            }

            moduleToRenderer.Add(module, mapCellOptionInstance);
            mapCellOptionInstance.OnCreated(module,this);


        }

    }

}
