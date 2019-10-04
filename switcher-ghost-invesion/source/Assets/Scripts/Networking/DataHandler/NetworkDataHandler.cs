public abstract class NetworkDataHandler {
    protected NetworkGameController gameController;

    public NetworkDataHandler(NetworkGameController gameController) {
        this.gameController = gameController;
    }

    public abstract void HandleMessage(string message);

}