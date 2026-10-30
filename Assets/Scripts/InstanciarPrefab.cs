
using UnityEngine;

public class InstanciarPrefab : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    [Header("Punto de instanciacion")]
    public Transform puntoInstanciacion;

    private GameObject figuraActual;
    private Vector3 escalaOriginal;
    private Color[] coloresOriginales;
    private Renderer[] renderersOriginales;

    // BOTON 1
    public void InstanciarFigura1()
    {
        Instanciar(prefab1);
    }

    // BOTON 2
    public void InstanciarFigura2()
    {
        Instanciar(prefab2);
    }

    // BOTON 3
    public void InstanciarFigura3()
    {
        Instanciar(prefab3);
    }

    // FUNCION GENERAL
    private void Instanciar(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Prefab no asignado");
            return;
        }

        if (figuraActual != null)
        {
            Destroy(figuraActual);
        }

        Vector3 posicion = puntoInstanciacion != null
            ? puntoInstanciacion.position
            : transform.position;

        Quaternion rotacion = puntoInstanciacion != null
            ? puntoInstanciacion.rotation
            : Quaternion.identity;

        figuraActual = Instantiate(prefab, posicion, rotacion);
        // Guardar tamaño original
        escalaOriginal = figuraActual.transform.localScale;

        // Guardar materiales originales
        renderersOriginales =
            figuraActual.GetComponentsInChildren<Renderer>();

        coloresOriginales = new Color[renderersOriginales.Length];

        for (int i = 0; i < renderersOriginales.Length; i++)
        {
            coloresOriginales[i] =
                renderersOriginales[i].material.color;
        }
    }

    // CAMBIAR COLOR
    public void CambiarColor(Color color)
    {
        if (figuraActual == null) return;

        Renderer[] renderers =
            figuraActual.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            r.material.color = color;
        }
    }

    public void ColorRojo()
    {
        CambiarColor(Color.red);
    }

    public void ColorAzul()
    {
        CambiarColor(Color.blue);
    }

    public void ColorVerde()
    {
        CambiarColor(Color.green);
    }

    public void TamanoChico()
    {
        CambiarTamano(0.5f);
    }

    public void TamanoMediano()
    {
        CambiarTamano(1f);
    }

    public void TamanoGrande()
    {
        CambiarTamano(1.5f);
    }

    private void CambiarTamano(float escala)
    {
        if (figuraActual == null)
        {
            Debug.LogWarning("Primero debes instanciar una figura.");
            return;
        }

        figuraActual.transform.localScale =
            Vector3.one * escala;
    }
    public void ResetFigura()
    {
        if (figuraActual == null)
        {
            Debug.LogWarning("No hay ninguna figura para resetear.");
            return;
        }

        // Restaurar tamaño original
        figuraActual.transform.localScale = escalaOriginal;

        // Restaurar colores originales
        for (int i = 0; i < renderersOriginales.Length; i++)
        {
            if (renderersOriginales[i] != null)
            {
                renderersOriginales[i].material.color =
                    coloresOriginales[i];
            }
        }
    }

}
