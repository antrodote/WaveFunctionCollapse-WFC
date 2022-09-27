using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///<summary>
///  Clase abstracta que representa un estado para una celda.
/// </summary>
public abstract class Module
{

    ///<summary>
    ///  Nombre del modulo.
    /// </summary>
    public string display;
    ///<summary>
    ///  Restriccion del modulo.
    /// </summary>
    public ModuleConstraint constraint;
    

    public override string ToString()
    {
        return display;
    }

    public override int GetHashCode()
    {
        return display.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (GetType().IsInstanceOfType(obj))
        {
            return ((Module)obj).display.Equals(this.display);
        }

        return false;
    }


}

///<summary>
///  Clase abstracta que representa un modulo 2D.
/// </summary>
public abstract class Module2D : Module {

    ///<summary>
    ///  Imagen del modulo.
    /// </summary>
    public Sprite sprite;

}


