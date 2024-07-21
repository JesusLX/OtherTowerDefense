using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class WallController : MonoBehaviour
{
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;

    [ContextMenu("ChangeForWall1")]
    public void ChangeForWall1() {
        Debug.Log("1");
        Instanciame(wall1);

    }
    [ContextMenu("ChangeForWall2")]
    public void ChangeForWall2() {
        Debug.Log("2");
        Instanciame(wall2);
    }
    [ContextMenu("ChangeForWall3")]
    public void ChangeForWall3() {
        Debug.Log("3");
        Instanciame(wall3);
    }
    public void Instanciame(GameObject go) {
        Instantiate(go, transform.position, transform.rotation);
        //DestroyImmediate(this.gameObject);
    }
}
