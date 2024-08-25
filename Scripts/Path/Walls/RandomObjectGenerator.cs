using System.Collections.Generic;
using UnityEngine;

public class RandomObjectGenerator : Singleton<RandomObjectGenerator>
{

    public List<GameObject> objects;

    public void InstantiateRandom(HoverPlatform platform) {
        var obj = objects[Random.Range(0, objects.Count)];
        var instObj = Instantiate(obj, platform.Position,GetRandomCardinalRotation());
        platform.SetOccupier(instObj);
    }
    public Quaternion GetRandomCardinalRotation() {
        // Crear una lista de los ángulos disponibles
        int[] angles = { 0, 90, 180, 270 };

        // Seleccionar un ángulo aleatorio de la lista
        int randomIndex = Random.Range(0, angles.Length);

        // Crear y devolver una rotación en el eje Y basada en el ángulo aleatorio
        return Quaternion.Euler(0, angles[randomIndex], 0);
    }
}
