using UnityEngine;

///<summary>
///  Clase encargada de gestionar el estado del juego a traves de eventos.
/// </summary>
public class GameStatusHandler : MonoBehaviour
{
    private GameStatus currentGameStatus;
    public delegate void GameStatusChangeHandler(GameStatus newGameStatus);
    private event GameStatusChangeHandler onGameStatusChanged;

     void Start()
    {
        this.currentGameStatus = GameStatus.Gameplay;    
    }

    public GameStatus GetGameStatus()
    {
        return this.currentGameStatus;
    }

    public void PauseGameplay()
    {
        this.SetGameStatus(GameStatus.Paused);
    }
    public void RenaudeGameplay()
    {
        this.SetGameStatus(GameStatus.Gameplay);
    }
    public void SetGameStatus(GameStatus newGameStatus)
    {
        if ( newGameStatus.Equals(currentGameStatus)) return;

        currentGameStatus = newGameStatus;
        onGameStatusChanged?.Invoke(newGameStatus);

    }
    public void SubscribeToGameStatusEvent(GameStatusChangeHandler gameStatusChangeHandler)
    {
        onGameStatusChanged += gameStatusChangeHandler;
    }
    public void UnSubscribeToGameStatusEvent(GameStatusChangeHandler gameStatusChangeHandler)
    {
        onGameStatusChanged -= gameStatusChangeHandler;
    }
}
