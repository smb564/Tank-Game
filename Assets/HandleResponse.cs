using UnityEngine;
using System.Collections;
using Assets;

public class HandleResponse
{

    public HandleResponse()
    {
        ServerConfig.Connection.getInstance().MessageReceiveEvent += new ServerConfig.MessageReceiveHandler(handleResponse);
    }

    void handleResponse(string response)
    {
        response = response.Substring(0,response.Length-1);

        // This handles the requests
        if (response.Substring(1, 1) == ":")
        {
            switch (response.Substring(0, 1))
            {
                case "S":
                    // Starting position
                    break;

                case "I":
                    createBoard(response);
                    break;

                case "G":
                    updateBoard(response);
                    break;

                case "C":
                    // Coins
                    break;

                case "L":
                    // lifepacks
                    break;

            }

        }
        else
        {
            switch (response)
            {
                case "OBSTACLE#":
                    break;

                case "CELL_OCCUPIED#":
                    break;

                case "DEAD#":
                    break;

                case "TOO_QUICK#":
                    break;

                case "INVALID_CELL#":
                    break;

                case "GAME_HAS_FINISHED#":
                    break;

                case "GAME_NOT_STARTED_YET#":
                    break;

                case "NOT_A_VALID_CONTESTANT":
                    break;

                default:
                    Debug.Log(response);
                    Debug.Log("Invalid Response ???");
                    break;

            }
        }
    }

    void updateBoard(string response)
    {
        string[] parts = response.Split(':');

        // starts from parts[1]
        for (int i = 1; i < parts.Length; i++)
        {
            string[] subParts = parts[i].Split(';');

            // Check whether a player or brick detail
            if (subParts[0].Substring(0, 1) == "P")
            {
                // Player details
                createPlayer(subParts[0], subParts[1], subParts[2], subParts[3], subParts[4], subParts[5], subParts[6]);
            }
            else
            {
                // Brick damage details
                updateBrickDamage(subParts[0], subParts[1], subParts[3]);
            }
        }
    }

    void createPlayer(string player, string cordinates, string direction, string shot, string health, string coins, string points)
    {
        // Update player locations on the map
        // Dispatch to main thread (Because some API funcitons can only be executed in Main Thread)
        player = "Player" + player.Substring(1, 1); // Player number
        Debug.Log(player);
        Dispatcher.getInstance().invoke(
            () =>
            {
                Debug.Log("Translate the player 1 into the middle");
                GameObject gameObject = GameObject.Find(player);
                gameObject.transform.position = new Vector3(float.Parse(cordinates.Split(',')[0]) - 4.5f, 4.5f - float.Parse(cordinates.Split(',')[1]), 0);
                gameObject.transform.Rotate(Constants.DIRECTION[int.Parse(direction)]);

                // Show shot, health, coins, points
            }
            );
    }

    void updateBrickDamage(string x, string y, string damageLevel)
    {
        // update brick damages
    }

    void createBoard(string response)
    {
        string[] parts = response.Split(':');

        // parts[1] is our clients player number; Store this!!!

        // Bricks
        createBricks(parts[2]);

        // Stones
        createStones(parts[3]);

        // Water
        createWater(parts[4]);

    }

    void createBricks(string bricks)
    {
        string[] brickCordinates = bricks.Split(';');

        Dispatcher.getInstance().invoke(
        () =>
        {
            foreach (string coordinates in brickCordinates)
            {
                float x = float.Parse(coordinates.Split(',')[0]) - 4.5f;
                float y = 4.5f - float.Parse(coordinates.Split(',')[1]);

                GameObject gameObject = GameObject.Find("Brick");
                // I just created objects since it is the start; But later on when updating damage there should be a way to track these objects!!!!
                GameObject clone = MonoBehaviour.Instantiate(gameObject);
                clone.transform.position = new Vector3(x, y, 0);
            }
        });

    }

    void createStones(string stones)
    {
        string[] stoneC = stones.Split(';');


        Dispatcher.getInstance().invoke(
        () =>
        {
            for (int i = 0; i < stoneC.Length; i++)
            {
                // Stones will never change positions, fixed.
                GameObject gameObject = GameObject.Find("Stone");
                GameObject clone = MonoBehaviour.Instantiate(gameObject);
                clone.transform.position = new Vector3(float.Parse(stoneC[i].Split(',')[0]) - 4.5f, 4.5f - float.Parse(stoneC[i].Split(',')[1]), 0);
            }
        });

    }
    void createWater(string water)
    {
        string[] waterC = water.Split(';');


        Dispatcher.getInstance().invoke(
        () =>
        {
            for (int i = 0; i < waterC.Length; i++)
            {
                GameObject gameObject = GameObject.Find("Water");
                // Water is fixed
                GameObject clone = MonoBehaviour.Instantiate(gameObject);
                clone.transform.position = new Vector3(float.Parse(waterC[i].Split(',')[0]) - 4.5f, 4.5f - float.Parse(waterC[i].Split(',')[1]), 0);
            }
        });

    }
}