using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public CoreData coreData;
    private Match _match;
    public Camera mainCamera;
    public LayerMask boardSlotLayerMask;
    public SeasonData seasonData;

    public void Start()
    {
        BankMusic();
        Board board = BoardGeneration.GenerateBoard(coreData, seasonData);
        _match = new Match(board, seasonData.Events, coreData.audioBank);
        _match.AddBehaviour(new MouseTracker(board, mainCamera, boardSlotLayerMask));
        _match.Enable();
    }

    public void Update()
    {
        _match.Update(Time.deltaTime);
    }

    private void BankMusic()
    {
        foreach (SeasonPacket seasonPacket in seasonData.seasonSprites)
        {
            AudioSource audioSource = Instantiate(seasonPacket.audioSource);
            coreData.audioBank.AddAudio(seasonPacket.season.ToString(), audioSource);
        }
    }
}
