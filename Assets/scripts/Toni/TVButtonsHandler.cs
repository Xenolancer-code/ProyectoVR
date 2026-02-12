using SFB;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class TVButtonsHandler : MonoBehaviour
{
    [Header("UI Reference")]
    public RawImage targetRawImage;       // Drag your RawImage here

    [Header("Settings")]
    public float imageHeight = 500f;      // RawImage height in pixels (width auto-adjusts to aspect)

    private List<Texture2D> images = new List<Texture2D>();
    private int currentIndex = 0;

    private bool isTVon = false;

    public VRButton3D[] buttons; // Array con los 4 botones
    public LayerMask buttonLayer; // Layer donde están los botones
    public float rayDistance = 10f;
    public Camera mainCamera;         // Cámara principal

    private VRButton3D currentButton;

    void Start()
    {
        targetRawImage.enabled = isTVon;

        // Asignar acciones a los botones
        if (buttons.Length >= 4)
        {
            buttons[0].onPressAction = () => TurnScreenOnOff();
            buttons[1].onPressAction = () => OpenFolderAndLoadImages();
            buttons[2].onPressAction = () => PreviousImage();
            buttons[3].onPressAction = () => NextImage();
        }

        // Si no asignaste cámara, toma la principal
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buttonLayer))
        {
            VRButton3D button = hit.collider.GetComponent<VRButton3D>();
            if (button != null)
            {
                currentButton = button;

                // Presionar al hacer click izquierdo
                if (Input.GetMouseButtonDown(0))
                {
                    currentButton.Press();
                }

                // Soltar al soltar el click
                if (Input.GetMouseButtonUp(0))
                {
                    currentButton.Release();
                }
            }
        }
    }

    public void OpenFolderAndLoadImages()
    {
        // Open folder dialog
        string[] folders = StandaloneFileBrowser.OpenFolderPanel("Select Image Folder", "", false);
        if (folders.Length > 0)
        {
            string folderPath = folders[0]; // take the first selected folder
            LoadAllImagesFromFolder(folderPath);
        }
    }

    private void LoadAllImagesFromFolder(string folderPath)
    {
        images.Clear();
        currentIndex = 0;

        // Get all image files
        string[] files = Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly);
        foreach (string file in files)
        {
            string ext = Path.GetExtension(file).ToLower();
            if (ext == ".png" || ext == ".jpg" || ext == ".jpeg")
            {
                byte[] bytes = File.ReadAllBytes(file);
                Texture2D tex = new Texture2D(2, 2);
                if (tex.LoadImage(bytes))
                {
                    images.Add(tex);
                }
            }
        }

        if (images.Count > 0)
        {
            DisplayImage(currentIndex);
        }
        else
        {
            Debug.LogWarning("No images found in folder: " + folderPath);
        }
    }

    /// <summary>
    /// Display image at index
    /// </summary>
    private void DisplayImage(int index)
    {
        if (images.Count == 0 || index < 0 || index >= images.Count) return;

        Texture2D tex = images[index];
        targetRawImage.texture = tex;

        // Adjust RawImage to match aspect ratio
        RectTransform rt = targetRawImage.GetComponent<RectTransform>();
        float aspect = (float)tex.width / tex.height;
        rt.sizeDelta = new Vector2(imageHeight * aspect, imageHeight);
    }

    /// <summary>
    /// Call this to show the next image (slide)
    /// </summary>
    public void NextImage()
    {
        if (images.Count == 0) return;

        currentIndex = (currentIndex + 1) % images.Count;
        DisplayImage(currentIndex);
    }

    /// <summary>
    /// Optional: previous image
    /// </summary>
    public void PreviousImage()
    {
        if (images.Count == 0) return;

        currentIndex = (currentIndex - 1 + images.Count) % images.Count;
        DisplayImage(currentIndex);
    }

    public void TurnScreenOnOff()
    {
        isTVon = !isTVon;
        targetRawImage.enabled = isTVon ? false : true;
    }
}
