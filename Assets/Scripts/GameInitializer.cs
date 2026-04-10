using UnityEngine;

public class GameInitializer: MonoBehaviour
{
    public CoreData coreData;
    private Match _match;
    
    public void Start()
    {
        Board board = BoardGeneration.GenerateBoard(coreData);
        _match = new Match(board, coreData.matchEvents);
        _match.Enable();	
    }

    public void Update()
    {
        _match.Update(Time.deltaTime);
    }
}
