using System.Collections.Generic;
using UnityEngine;

public class TowerListUI : MonoBehaviour
{
    public GameObject towerButtonPrefab;
    public Transform panelList;
    public TowerManager towerManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(towerManager == null) {
            towerManager = TowerManager.Instance;
        }
        FillActiveList();
    }
    public void FillActiveList() {
        RemoveAllChildren(panelList);
        List<TowerController.TowerInfo> towers = towerManager.GetActiveTowers();
        foreach (var towerInfo in towers) {
            var towerButton = Instantiate(towerButtonPrefab, panelList);
            towerButton.GetComponent<BuyTowerButton>().SetInfo(towerInfo);
        }
    }
    public void RemoveAllChildren(Transform panel) {
        // Iterar sobre todos los hijos y destruirlos
        foreach (Transform child in panel) {
            Destroy(child.gameObject);
        }
    }
}
