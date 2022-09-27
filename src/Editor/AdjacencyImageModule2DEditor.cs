using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

///<summary>
///  Clase para generacion de un editor customizado en configuraciones de tipo AdjacencyImageModule2DConfig.
/// </summary>
[CustomEditor(typeof(AdjacencyImageModule2DConfig))]
public class AdjacencyImageModule2DEditor : Editor
{
    int currentPickerWindow;

    public override void OnInspectorGUI()
    {
        AdjacencyImageModule2DConfig config = target as AdjacencyImageModule2DConfig;
        if (config == null) return;

        Rect totalVerticalRect = EditorGUILayout.BeginVertical(GUILayout.Height(100));


        DisplayMapGridWidthParameter(config);
        DisplayMapGridHeightParameter(config);
        DisplayMapGridWrapperParameter(config);
        ClearAllSpriteHandleButton(config);
        AddFromSpriteSheetHandleButton(config);
        LoadSpritesEventHandler(config);


        EditorGUILayout.EndVertical();


        DisplayConfigurationModules(config);
        AddNewModuleHandleButton(config);


    }

    
    private void DisplayMapGridWrapperParameter(AdjacencyImageModule2DConfig config)
    {

        SerializedProperty wrapperConstraint = serializedObject.FindProperty("wrapper");
        EditorGUI.BeginChangeCheck();
        if (EditorGUILayout.PropertyField(wrapperConstraint, true) && EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change wrapper");
            EditorUtility.SetDirty(config);
        }

    }


    private void DisplayMapGridWidthParameter(AdjacencyImageModule2DConfig config)
    {

        EditorGUI.BeginChangeCheck();
        var nextWidth = EditorGUILayout.IntField("Width", config.size.x);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change width");
            config.size.x = nextWidth;
            EditorUtility.SetDirty(config);
        }

    }

    private void DisplayMapGridHeightParameter(AdjacencyImageModule2DConfig config)
    {

        EditorGUI.BeginChangeCheck();
        var nextHeight = EditorGUILayout.IntField("Height", config.size.y);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change height");
            config.size.y = nextHeight;
            EditorUtility.SetDirty(config);
        }

    }


    private void ClearAllSpriteHandleButton(AdjacencyImageModule2DConfig config)
    {


        if (GUILayout.Button("Clear All Sprites"))
        {
            Undo.RecordObject(target, "Clear all sprites");
            config.modules.Clear();
            config.constraints.Clear();
            EditorUtility.SetDirty(config);

        }

    }


    private void AddFromSpriteSheetHandleButton(AdjacencyImageModule2DConfig config)
    {


        if (GUILayout.Button("Add from Sprite Sheet"))
        {
            Texture2D txt = null;
            //create a window picker control ID
            currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive) + 100;
            EditorGUIUtility.ShowObjectPicker<Texture2D>(txt, false, "", currentPickerWindow);

        }

    }


    private void LoadSpritesEventHandler(AdjacencyImageModule2DConfig config)
    {

        if (Event.current.commandName == "ObjectSelectorUpdated" &&
            EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
        {
            Undo.RecordObject(target, "Set sprite");

            var txt = EditorGUIUtility.GetObjectPickerObject() as Texture2D;
            Debug.Log("Texture " + txt.name);
            string spriteSheet = AssetDatabase.GetAssetPath(txt);
            Debug.Log("Loading sprite sheet at path " + spriteSheet);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet)
                .OfType<Sprite>().ToArray();
            Debug.Log("Found " + sprites.Length + " sprites");


            foreach (Sprite sprite in sprites)
            {
                config.modules.Add(new AdjacencyImageModule2D(sprite, null));
                List<Sprite> validLeftNeighbors = new List<Sprite>();
                List<Sprite> validRightNeighbors = new List<Sprite>();
                List<Sprite> validTopNeighbors = new List<Sprite>();
                List<Sprite> validBottomNeighbors = new List<Sprite>();
                config.constraints.Add(new AdjacencyImageConstraint2D(sprite, validLeftNeighbors, validRightNeighbors, validTopNeighbors, validBottomNeighbors));

            }
            currentPickerWindow = -1;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(config);

        }

    }


    public void DisplayConfigurationModules(AdjacencyImageModule2DConfig config)
    {


        var trueHeight = (Screen.height - 200);

        int toDelete = -1;

        for (var i = 0; i < config.modules.Count; i++)
        {
            AdjacencyImageModule2D module = config.modules[i];
            if (string.IsNullOrEmpty(module.display))
            {
                module.display = module.sprite == null ? null : module.sprite.name;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(module.display);

            if (GUILayout.Button("Remove Module", GUILayout.Width(200)))
            {
                toDelete = i;

            }
            EditorGUILayout.EndHorizontal();


            EditorGUI.BeginChangeCheck();
            var nextSprite = EditorGUILayout.ObjectField("Sprite", module.sprite, typeof(Sprite), false) as Sprite;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change sprite");
                module.sprite = nextSprite;
                module.display = module.sprite.name;
                EditorUtility.SetDirty(target);
            }


            Texture2D texture = AssetPreview.GetAssetPreview(module.sprite);

            var lastRect = GUILayoutUtility.GetLastRect();
            EditorGUILayout.Space(25);

            var previewRect = new Rect(lastRect.x + 25, lastRect.yMax + 25, 100, 100);

            var lastY = previewRect.yMin;
            var lastX = previewRect.xMin;
            if (texture != null)
            {
                EditorGUI.DrawPreviewTexture(previewRect, texture, null, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.Space(25);


            EditorGUILayout.Space(100);

            if (module.sprite == null)
            {
                EditorGUI.LabelField(previewRect, "<no sprite>");
            }
            else
            {

                
                AdjacencyImageConstraint2D moduleAdjacencyConstraint = config.constraints[i];
                

                var leftRect = new Rect(lastRect.x, lastY, 20, 10);
                var rightRect = new Rect(previewRect.xMax + 12, lastY, 20, 10);

                var lowRect = new Rect(lastX, previewRect.yMin - 25, 15, 20);
                var topRect = new Rect(lastX, previewRect.yMax + 5, 15, 20);


                SerializedProperty constraints = serializedObject.FindProperty("constraints");
                SerializedProperty constraint = constraints.GetArrayElementAtIndex(i);
                
                EditorGUI.BeginChangeCheck();
                
                
                if (EditorGUILayout.PropertyField(constraint, true) && EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Change Valid Left Neighbors");
                    EditorUtility.SetDirty(config);
                }



            }

            EditorGUILayout.Space(12);
            Rect rect = EditorGUILayout.GetControlRect(false, 1);

            rect.height = 1;

            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
            EditorGUILayout.Space(12);

        }


        if (toDelete != -1)
        {
            Undo.RecordObject(target, "Remove module");
            config.modules.RemoveAt(toDelete);
            config.constraints.RemoveAt(toDelete);
            toDelete = -1;
            EditorUtility.SetDirty(target);
        }





    }


    private void AddNewModuleHandleButton(AdjacencyImageModule2DConfig config)
    {

        if (GUILayout.Button("Add new Module"))
        {
            Undo.RecordObject(target, "Add module");
            config.modules.Add(new AdjacencyImageModule2D(null, null));
            List<Sprite> validLeftNeighbors = new List<Sprite>();
            List<Sprite> validRightNeighbors = new List<Sprite>();
            List<Sprite> validTopNeighbors = new List<Sprite>();
            List<Sprite> validBottomNeighbors = new List<Sprite>();
            config.constraints.Add(new AdjacencyImageConstraint2D(null, validLeftNeighbors,validRightNeighbors, validTopNeighbors, validBottomNeighbors));
            EditorUtility.SetDirty(target);
        }

    }

}
