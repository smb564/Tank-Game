using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;

public class ServerConnection : MonoBehaviour {
    ServerConfig.Connection connection;
    void Awake()
    {
        // Get the connection
        connection = ServerConfig.Connection.getInstance();
        connection.startListening();
        HandleResponse handler = new HandleResponse();
        Debug.Log("Connection Loaded!");
    }

    void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("U")))
        {
            connection.sendData("UP#");
            Debug.Log("U pressed");
        }


        if (Event.current.Equals(Event.KeyboardEvent("J")))
        {
            connection.sendData("JOIN#");
            Debug.Log("J pressed");
        }

        if (Event.current.Equals(Event.KeyboardEvent("L")))
        {
            connection.sendData("LEFT#");
            Debug.Log("L pressed");
        }

        if (Event.current.Equals(Event.KeyboardEvent("R")))
        {
            connection.sendData("RIGHT#");
            Debug.Log("R pressed");
        }

        if (Event.current.Equals(Event.KeyboardEvent("D")))
        {
            connection.sendData("DOWN#");
            Debug.Log("D pressed");
        }

        if (Event.current.Equals(Event.KeyboardEvent("S")))
        {
            connection.sendData("SHOOT#");
            Debug.Log("S pressed");
        }
    }
}
