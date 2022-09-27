using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>
///  Clase que nos permite observar el proceso del algoritmo WFC mediante eventos.
/// </summary>
public class WFCProgressObserver
{
    //Delegados
    public delegate void OnProgressHandler(WFCProgress progress);
    public delegate void OnCompletedHandler();
    public delegate void OnSelectedModuleHandler(WFCMapCellCollapsed selection);

    //Eventos
    public event OnProgressHandler onProgress;
    public event OnCompletedHandler onComplete;
    public event OnSelectedModuleHandler onSelectedModule;

    //Notificadores
    public void NotifyOnProgress(WFCProgress progress) {
        onProgress?.Invoke(progress);
        switch (progress) {

            case WFCMapCellCollapsed selection: 
                
                NotifyOnSelectedModule(selection);
                break;

            default: break;
        }
    }

    public void NotifyOnComplete() {
        onComplete?.Invoke();
    }
    public void NotifyOnSelectedModule(WFCMapCellCollapsed selection) {
        onSelectedModule?.Invoke(selection);
    }

}
