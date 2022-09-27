using UnityEngine;

///<summary>
///  Clase abstracta que representa una restricci�n a cumplir por un modulo
/// </summary>
public abstract class ModuleConstraint 
{

    ///<summary>
    ///  M�todo que nos da informaci�n si dos modulos son compatibles entre si a trav�s de sus restricci�nes en un determinado espacio.
    /// </summary>
    /// <param name="otherModuleConstraint">Restricci�n de otro modulo</param>
    /// <param name="thisModuleConstraintCoords">Las coordenadas del modulo que tiene que cumplir una restricci�n</param>
    /// <param name="otherModuleConstraintCoords">Las coordenadas del modulo asociada a la restriccion otherModuleConstraint</param>
    /// <returns>Devuelve un booleano que indica si la restricciones de dos modulos son compatibles entre si</returns>
    public abstract bool ShouldRemoveModule<TModuleConstraint>(TModuleConstraint otherModuleConstraint,Vector3Int thisModuleConstraintCoords
        ,Vector3Int otherModuleConstraintCoords) where TModuleConstraint : ModuleConstraint;

}
