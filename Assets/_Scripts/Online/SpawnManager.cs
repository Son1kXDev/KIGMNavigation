using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviour
{
    public GameObject SpawnPoint;

    public GameObject[] PlayersPrefab;

    private void Awake()
    {
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter");
        PhotonNetwork.Instantiate(PlayersPrefab[selectedCharacter].name, SpawnPoint.transform.localPosition, Quaternion.identity);
    }
}