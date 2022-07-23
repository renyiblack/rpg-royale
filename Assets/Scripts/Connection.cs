using WebSocketSharp;

public sealed class Connection
{
    private Connection()
    {
        ws = new WebSocket("ws://localhost:8081");
        ws.Connect();
    }

    private static Connection _instance;

    public WebSocket ws;

    public static Connection GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Connection();
        }
        return _instance;
    }

    public void someBusinessLogic()
    {
    }
}