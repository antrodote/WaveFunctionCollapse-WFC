using System.Collections.Generic;
using UnityEngine;
using System.Linq;


///<summary>
///  Clase abstracta que define una configuración sobre la que realizar el algoritmo WFC.
/// </summary>
public abstract class WFConfigObject : ScriptableObject
{
    ///<summary>
    ///  Metodo que nos permite generar un entorno de ejecucion del algoritmo WFC mediante una configuracion.
    /// </summary>
    /// <returns>Develve un WFCRunner sobre el cual ejecutar el algoritmo WFC</returns>
    public abstract WFCRunner CreateSpace();   
}

///<summary>
///  Clase abstracta que define una configuración sobre un tipo de modulo, restriccion y cuadricula.
/// </summary>
public abstract class WFConfigObject<TModule,TModuleConstraint,TMapGrid> : WFConfigObject where TModule : Module 
    where TMapGrid : MapGrid where TModuleConstraint: ModuleConstraint
{

    ///<summary>
    ///  Modulos de la configuracion.
    /// </summary>
    [SerializeField]
    public List<TModule> modules = new List<TModule>();


    ///<summary>
    ///  Restricciones de los modulos de la configuracion.
    /// </summary>
    [SerializeField]
    public List<TModuleConstraint> constraints = new List<TModuleConstraint>();


    ///<summary>
    ///  Metodo para obtener los modulos de la configuracion.
    /// </summary>
    /// <returns>Una lista de modulos</returns>
    public virtual List<TModule> GetModules() {

        return modules;
       
    }

    ///<summary>
    ///  Metodo para obtener las restricciones de los modulos de la configuracion.
    /// </summary>
    /// <returns>Una lista de restricciones</returns>
    public virtual List<TModuleConstraint> GetConstraints()
    {
        return constraints;
    }

    ///<summary>
    ///  Metodo que genera la funcion de onda bajo el algoritmo WFC.
    /// </summary>
    /// <returns>Un WFCRoutines sobre una cuadricula especifica</returns>
    public abstract WFCRoutines<TMapGrid> CreateWFC();

    public override WFCRunner CreateSpace()
    {

        WFCRoutines<TMapGrid> wfcSolver = this.CreateWFC();
        return wfcSolver;

    }


}


