#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using NewMaster.Bootstrap;
using NewMaster.Core;
using NewMaster.UI;
using NewMaster.Worlds;

namespace NewMaster.Editor
{
    public static class NewMasterSceneGenerator
    {
        private const string SceneFolder = "Assets/NewMaster/Scenes";
        private const string ScenePath = SceneFolder + "/NewMaster_Boot.unity";
        private const string CatalogPath = "Assets/NewMaster/Content/Worlds/WorldCatalog.asset";

        [MenuItem("New Master/Scenes/Generate Production Boot Scene")]
        public static void Generate()
        {
            EnsureFolder("Assets/NewMaster");
            EnsureFolder(SceneFolder);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraObject = new GameObject("Main Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.tag = "MainCamera";
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.035f, 0.03f, 0.045f);

            var bootstrapObject = new GameObject("New Master Bootstrap");
            bootstrapObject.AddComponent<NewMasterBootstrap>();
            var engine = bootstrapObject.GetComponent<GameEngine>();

            var catalog = AssetDatabase.LoadAssetAtPath<WorldCatalog>(CatalogPath);
            if (catalog != null)
            {
                var serializedEngine = new SerializedObject(engine);
                serializedEngine.FindProperty("worldCatalog").objectReferenceValue = catalog;
                serializedEngine.ApplyModifiedPropertiesWithoutUndo();
            }

            var canvasObject = new GameObject("UI", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            var safeObject = CreateChild("Safe Area", canvasObject.transform);
            var safeArea = safeObject.AddComponent<NewMasterSafeArea>();
            var safeRect = safeObject.GetComponent<RectTransform>();
            safeRect.anchorMin = Vector2.zero;
            safeRect.anchorMax = Vector2.one;
            safeRect.offsetMin = Vector2.zero;
            safeRect.offsetMax = Vector2.zero;

            var hudObject = CreateChild("HUD", safeObject.transform);
            var hud = hudObject.AddComponent<NewMasterHud>();
            var hudRect = hudObject.GetComponent<RectTransform>();
            hudRect.anchorMin = Vector2.zero;
            hudRect.anchorMax = Vector2.one;
            hudRect.offsetMin = Vector2.zero;
            hudRect.offsetMax = Vector2.zero;

            var coins = CreateText("Coins", hudObject.transform, new Vector2(0, 0.93f), new Vector2(0.48f, 1f));
            var energy = CreateText("Energy", hudObject.transform, new Vector2(0.52f, 0.93f), new Vector2(1f, 1f));
            var world = CreateText("World", hudObject.transform, new Vector2(0, 0.84f), new Vector2(0.48f, 0.91f));
            var village = CreateText("Village", hudObject.transform, new Vector2(0.52f, 0.84f), new Vector2(1f, 0.91f));
            var status = CreateText("Status", hudObject.transform, new Vector2(0.08f, 0.72f), new Vector2(0.92f, 0.81f));

            var slots = new TMP_Text[3];
            for (var i = 0; i < 3; i++)
            {
                var min = new Vector2(0.08f + i * 0.29f, 0.47f);
                var max = new Vector2(0.32f + i * 0.29f, 0.67f);
                slots[i] = CreateText($"Slot {i + 1}", hudObject.transform, min, max);
                slots[i].fontSize = 72;
                slots[i].alignment = TextAlignmentOptions.Center;
            }

            var spinButtonObject = CreateButton("SPIN", hudObject.transform, new Vector2(0.2f, 0.25f), new Vector2(0.8f, 0.39f));
            var spinButton = spinButtonObject.GetComponent<Button>();

            var socialObject = CreateChild("Guild Co-op", safeObject.transform);
            var socialRect = socialObject.GetComponent<RectTransform>();
            socialRect.anchorMin = new Vector2(0.05f, 0.01f);
            socialRect.anchorMax = new Vector2(0.95f, 0.22f);
            socialRect.offsetMin = Vector2.zero;
            socialRect.offsetMax = Vector2.zero;

            var socialPanel = socialObject.AddComponent<NewMasterSocialPanel>();
            var guildName = CreateText("Guild", socialObject.transform, new Vector2(0.02f, 0.72f), new Vector2(0.48f, 1f));
            var memberCount = CreateText("Members", socialObject.transform, new Vector2(0.52f, 0.72f), new Vector2(0.98f, 1f));
            var scoreText = CreateText("Co-op Score", socialObject.transform, new Vector2(0.02f, 0.45f), new Vector2(0.48f, 0.70f));
            var contributionText = CreateText("Contribution", socialObject.transform, new Vector2(0.52f, 0.45f), new Vector2(0.98f, 0.70f));
            var milestoneText = CreateText("Next Milestone", socialObject.transform, new Vector2(0.02f, 0.22f), new Vector2(0.48f, 0.44f));
            var resultText = CreateText("Social Result", socialObject.transform, new Vector2(0.52f, 0.22f), new Vector2(0.98f, 0.44f));

            var contributeButtonObject = CreateButton("CONTRIBUTE", socialObject.transform, new Vector2(0.02f, 0.01f), new Vector2(0.48f, 0.20f));
            var claimButtonObject = CreateButton("CLAIM", socialObject.transform, new Vector2(0.52f, 0.01f), new Vector2(0.98f, 0.20f));

            var serializedSocial = new SerializedObject(socialPanel);
            serializedSocial.FindProperty("gameEngine").objectReferenceValue = engine;
            serializedSocial.FindProperty("guildNameText").objectReferenceValue = guildName;
            serializedSocial.FindProperty("memberText").objectReferenceValue = memberCount;
            serializedSocial.FindProperty("scoreText").objectReferenceValue = scoreText;
            serializedSocial.FindProperty("contributionText").objectReferenceValue = contributionText;
            serializedSocial.FindProperty("milestoneText").objectReferenceValue = milestoneText;
            serializedSocial.FindProperty("resultText").objectReferenceValue = resultText;
            serializedSocial.FindProperty("contributeButton").objectReferenceValue =
                contributeButtonObject.GetComponent<Button>();
            serializedSocial.FindProperty("claimMilestoneButton").objectReferenceValue =
                claimButtonObject.GetComponent<Button>();
            serializedSocial.ApplyModifiedPropertiesWithoutUndo();

            var serializedHud = new SerializedObject(hud);
            serializedHud.FindProperty("gameEngine").objectReferenceValue = engine;
            serializedHud.FindProperty("coinsText").objectReferenceValue = coins;
            serializedHud.FindProperty("energyText").objectReferenceValue = energy;
            serializedHud.FindProperty("worldText").objectReferenceValue = world;
            serializedHud.FindProperty("villageText").objectReferenceValue = village;
            serializedHud.FindProperty("statusText").objectReferenceValue = status;
            serializedHud.FindProperty("spinButton").objectReferenceValue = spinButton;

            var slotProperty = serializedHud.FindProperty("slotTexts");
            slotProperty.arraySize = slots.Length;
            for (var i = 0; i < slots.Length; i++)
                slotProperty.GetArrayElementAtIndex(i).objectReferenceValue = slots[i];

            serializedHud.ApplyModifiedPropertiesWithoutUndo();

            Selection.activeGameObject = bootstrapObject;
            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"New Master: generated boot scene at {ScenePath}");
        }

        private static TMP_Text CreateText(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
        {
            var objectRoot = CreateChild(name, parent);
            var rect = objectRoot.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = new Vector2(24, 12);
            rect.offsetMax = new Vector2(-24, -12);

            var text = objectRoot.AddComponent<TextMeshProUGUI>();
            text.text = name;
            text.fontSize = 38;
            text.alignment = TextAlignmentOptions.Center;
            return text;
        }

        private static GameObject CreateButton(string label, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
        {
            var objectRoot = CreateChild(label + " Button", parent);
            var rect = objectRoot.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var image = objectRoot.AddComponent<Image>();
            image.color = new Color(0.18f, 0.12f, 0.25f);

            var button = objectRoot.AddComponent<Button>();
            var text = CreateText("Label", objectRoot.transform, Vector2.zero, Vector2.one);
            text.text = label;
            text.fontSize = 52;
            return objectRoot;
        }

        private static GameObject CreateChild(string name, Transform parent)
        {
            var objectRoot = new GameObject(name, typeof(RectTransform));
            objectRoot.transform.SetParent(parent, false);
            return objectRoot;
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parent = System.IO.Path.GetDirectoryName(path)?.Replace("\\", "/");
            var folder = System.IO.Path.GetFileName(path);

            if (!string.IsNullOrEmpty(parent) && !AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);

            AssetDatabase.CreateFolder(parent, folder);
        }
    }
}
#endif
