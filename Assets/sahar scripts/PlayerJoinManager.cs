using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;

    private int currnetPlayerIndex = 0;

    private void Awake()
    {
        
        
    }

    private void Start()
    {
           if (currnetPlayerIndex == 0)
        {
            PlayerInputManager.instance.playerPrefab = player1Prefab;
            currnetPlayerIndex++;
        }
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoin;
    }

    private void OnPlayerJoin(PlayerInput input)
    {
        if (currnetPlayerIndex == 1)
        {
            PlayerInputManager.instance.playerPrefab = player2Prefab;
        }
    }
}
   
