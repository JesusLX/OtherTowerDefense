using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BuyTowerButton : MonoBehaviour {
    public string id;
    public string publicName;
    public int price;
    public float radius;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    public Image image;
    public Button button;
    public UnityEvent<GameObject> onElementClicked;

    private void Start() {
        Init();
    }
    public void Init() {
        SetPrice(price);
        SetName(publicName);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ButtonClicked);
    }

    public void SetImage(Sprite sprite) {
        image.sprite = sprite;
    }
    public void SetPrice(int price) {
        this.price = price;
        priceText.text = price.ToString() + "$";
    }
    public void SetName(string name) {
        this.publicName = name;
        nameText.text = name;
    }
    public void SetRadius(float radius) {
        this.radius = radius;
    }
    public void SetId(string id) {
        this.id = id;
    }
    public void SetInfo(TowerController.TowerInfo info) {
        SetId(info.id);
        SetName(info.name);
        SetPrice(info.price);
        SetImage(info.image);
        SetRadius(info.range);
    }

    public void ButtonClicked() {
        HoverController.Instance.Activate();
        HoverController.Instance.SetRadius(radius);
        HoverController.Instance.onObjectClicked.RemoveAllListeners();
        HoverController.Instance.onObjectClicked.AddListener(BuyTower);
    }
    public void BuyTower(GameObject target) {
       
           GameObject tower = TowerManager.Instance.GetTower(id);
            PlaceAbove(tower, target);
    }
    void PlaceAbove(GameObject objectToPlace, GameObject targetObject) {
        // Obtener el MeshRenderer del objeto objetivo
        HoverPlatform targetRenderer = targetObject.GetComponent<HoverPlatform>();
        if (targetRenderer == null) {
            Debug.LogError("El objeto objetivo no tiene un MeshRenderer.");
            return;
        }

     

        // Obtener la posición del objeto objetivo
        Vector3 targetPosition = targetObject.transform.position;

        // Calcular la nueva posición para colocar el objeto justo por encima del objeto objetivo
        Vector3 newPosition = targetPosition;
        newPosition.y += targetRenderer.Height;

        // Obtener el MeshRenderer del objeto a colocar
        MeshRenderer placeRenderer = objectToPlace.GetComponent<MeshRenderer>();
        if (placeRenderer != null) {
            // Obtener las extensiones del objeto a colocar
            Vector3 placeExtents = placeRenderer.bounds.extents;

            // Ajustar la nueva posición para tener en cuenta la altura del objeto a colocar
            newPosition.y += placeExtents.y;
        }

        // Colocar el objeto en la nueva posición
        objectToPlace.transform.position = newPosition;
    }

}
