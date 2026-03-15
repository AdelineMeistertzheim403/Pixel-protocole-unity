// Auto-generated from editorUtils.ts
// Conversion TypeScript -> C# pour Unity
using System;
using System.Collections.Generic;
using UnityEngine;

public static class EditorUtils
{
    public class EditorIssue
    {
        public string message;
        public string platformId;
        public string severity; // "error" ou "warning"
    }

    public class PlatformRenderData
    {
        public PlatformDef platform;
        public List<Rect> blocks;
        public Bounds? bounds;
        public Vector2? center;
    }

    public class ReachabilityLink
    {
        public object from; // { kind: "spawn" } ou { kind: "platform", platformId }
        public object to;   // { platformId }
    }

    public class PlatformValidation
    {
        public bool isValid;
        public List<EditorIssue> issues;
        public List<string> reachablePlatformIds;
        public List<ReachabilityLink> links;
    }

    // Ajoute ici les fonctions de validation, simulation, etc.
}
