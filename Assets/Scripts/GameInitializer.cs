using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public CoreData coreData;
    private Match _match;
    public Camera mainCamera;
    public LayerMask boardSlotLayerMask;

    public void Start()
    {
        Board board = BoardGeneration.GenerateBoard(coreData);
        _match = new Match(board, coreData.matchEvents);
        _match.AddBehaviour(new MouseTracker(board, mainCamera, boardSlotLayerMask));
        _match.Enable();
    }

    public void Update()
    {
        _match.Update(Time.deltaTime);
    }
}
