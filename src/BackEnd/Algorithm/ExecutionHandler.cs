using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
///  Clase estatica para los modos de ejecucion del algoritmo WFC.
/// </summary>
public static class ExecutionHandler 
{

    
    ///<summary>
    ///  Metodo para ejecutar el algoritmo WFC bajo las corrutinas de Unity y distribuir su ejecucion en distintos frames.
    /// </summary>
    /// <param name="operation">Cualquier operacion del algoritmo WFC.</param>
    /// <param name="context">Some Monobehaviour to run the Coroutine in. Algun Monobehaviour para lanzar la corrutina</param>
    /// <param name="timeBudgetPerFrame">¿Cuanto tiempo por frame hay disponible para operar con el algoritmo WFC? Este tiempo debe ser un numero pequeño para evitar perjudicar el rendimiento del juego.</param>
    /// <returns>Un WFCProgressObserver para poder subscribirse a eventos ocurridos por operaciones del algoritmo WFC</returns>
    public static WFCProgressObserver RunAsCoroutine(this IEnumerable<WFCProgress> operation, MonoBehaviour context, float timeBudgetPerFrame = .1f) {


        WFCProgressObserver wfcProgressObserver = new WFCProgressObserver();
       

        IEnumerator Routine() {

            yield return new WaitForEndOfFrame(); // Da la oportunidar al llamador de registrar metodos para los eventos generados
            var t = Time.realtimeSinceStartup;
            
            foreach (WFCProgress progress in operation) {

                wfcProgressObserver.NotifyOnProgress(progress);
               
                var d = Time.realtimeSinceStartup - t;
                if (d > timeBudgetPerFrame) {

                    yield return new WaitForEndOfFrame();
                    t = Time.realtimeSinceStartup;
                }

            }

            wfcProgressObserver.NotifyOnComplete();
            
        
        }


        Coroutine coroutine = context.StartCoroutine(Routine());
        return wfcProgressObserver;
        

    }

    ///<summary>
    ///  Metodo para ejecutar el algoritmo WFC en un frame a traves de iteraciones. Esto metodo problamente causara lag en tu juego y es solo recomendable para testeo o uso avanzado
    /// </summary>
    /// <param name="operation">Cualquier operacion del algoritmo WFC.</param>
    /// <param name="progressHandler">Un callback opcional que es invocado en cada proceso del algoritmo WFC.</param>
    public static void RunAsRoutine(this IEnumerable<WFCProgress> operation, Action<WFCProgress> progressHandler = null) {

        foreach (WFCProgress progress in operation) {

            progressHandler?.Invoke(progress);

        }
    
    }

}

