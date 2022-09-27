using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


///<summary>
///  Clase que representa una restricción de adjacencia de una imagen respecto a otras.
/// </summary>
[System.Serializable]
public class AdjacencyImageConstraint2D : ModuleConstraint
{


    ///<summary>
    ///  Imagen sobre la que se aplica la restricción de adjacencia.
    /// </summary>
    public Sprite sprite;

    ///<summary>
    ///  Imagenes compatibles a la izquierda.
    /// </summary>
    public List<Sprite> validLeftNeighbors;

    ///<summary>
    ///  Imagenes compatibles a la derecha.
    /// </summary>
    public List<Sprite> validRightNeighbors;

    ///<summary>
    ///  Imagenes compatibles arriba.
    /// </summary>
    public List<Sprite> validTopNeighbors;

    ///<summary>
    ///  Imagenes compatibles abajo.
    /// </summary>
    public List<Sprite> validBottomNeighbors;

    ///<summary>
    ///  Constructor de restricción de adjacencia con imagenes.
    /// </summary>
    /// <param name="sprite">Imagen de la restricción</param>
    /// <param name="validLeftNeighbors">Lista de imagenes compatibles a la izquierda.</param>
    /// <param name="validRightNeighbors">Lista de imagenes compatibles a la izquierda.</param>
    /// <param name="validTopNeighbors">Lista de imagenes compatibles arriba.</param>
    /// <param name="validBottomNeighbors">Lista de imagenes compatibles abajo.</param>
    public AdjacencyImageConstraint2D(Sprite sprite ,List<Sprite> validLeftNeighbors, List<Sprite> validRightNeighbors, 
        List<Sprite> validTopNeighbors, List<Sprite> validBottomNeighbors)
    {

        this.sprite = sprite;
        this.validLeftNeighbors = validLeftNeighbors;
        this.validRightNeighbors = validRightNeighbors;
        this.validTopNeighbors = validTopNeighbors;
        this.validBottomNeighbors = validBottomNeighbors;
    }

    public override bool ShouldRemoveModule<TModuleConstraint>(TModuleConstraint otherModuleConstraint, Vector3Int thisModuleConstraintCoords, 
        Vector3Int otherModuleConstraintCoords)
    {

        AdjacencyImageConstraint2D otherAdjacencyImageConstraint2D = otherModuleConstraint as AdjacencyImageConstraint2D;
        if (otherAdjacencyImageConstraint2D == null) return true;

        bool shouldRemoveOtherModule = false;


        if (thisModuleConstraintCoords.x > otherModuleConstraintCoords.x)

            shouldRemoveOtherModule = !this.validLeftNeighbors.Contains(otherAdjacencyImageConstraint2D.sprite);
            
        else if (thisModuleConstraintCoords.x < otherModuleConstraintCoords.x)
 
            shouldRemoveOtherModule = !this.validRightNeighbors.Contains(otherAdjacencyImageConstraint2D.sprite);
            
        else if (thisModuleConstraintCoords.y > otherModuleConstraintCoords.y)

            shouldRemoveOtherModule = !this.validBottomNeighbors.Contains(otherAdjacencyImageConstraint2D.sprite);
        
        else if (thisModuleConstraintCoords.y < otherModuleConstraintCoords.y)

            shouldRemoveOtherModule = !this.validTopNeighbors.Contains(otherAdjacencyImageConstraint2D.sprite);

        
        return shouldRemoveOtherModule;

        
    }



}
