using UnityEngine;
using twoDProject.Dungeon;

public class DungeonScene : MonoBehaviour
{
    public DungeonGenerator dungeonGenerator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (dungeonGenerator != null)
        {
            SoundManager.Instance.PlayBgm(EBGM.BGM_DUNGEON);

            dungeonGenerator.GenerateDungeon(() =>
            {
                GameManager.Instance.Player.transform.position = dungeonGenerator.PlayerStartPosition;
                UIManager.Instance.FadeOut();
            });
        }
    }
}
