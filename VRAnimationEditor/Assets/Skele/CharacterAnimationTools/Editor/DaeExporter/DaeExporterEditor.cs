using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using MH;

public class DaeExporterEditor : EditorWindow
{
	#region "data"
    // common
    private Vector3 m_ScrollPos = Vector2.zero;

    // SMR
    private List<AnimationClip> m_Clips = new List<AnimationClip>();
    private Transform m_RootBone;
    private List<SkinnedMeshRenderer> m_SMRs = new List<SkinnedMeshRenderer>();

    // MF
    private List<MeshFilter> m_MFs = new List<MeshFilter>();

    // misc
    private bool m_ForceIgnoreRootCurve = false;
    private List<AnimationClip> m_ClipsWithRootCurve = new List<AnimationClip>();

    private Texture2D _questionMark;

    #endregion "data"

	#region "public method"
    // public method

    [MenuItem("Window/Skele/DAE Exporter")]
    public static void Init()
    {
        var wnd = (DaeExporterEditor)GetWindow(typeof(DaeExporterEditor));
        EUtil.SetEditorWindowTitle(wnd, "DAE Exporter");

        // init
        wnd._questionMark = AssetDatabase.LoadAssetAtPath(MH.Skele.CAT.ResPath.QuestionMark, typeof(Texture2D)) as Texture2D;

        // auto-init
        wnd._AutoInit();

    }


    void OnGUI()
    {
        m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);
        _OnGUI_SMR();
        GUILayout.EndScrollView();

        Rect rbCorner = new Rect(position.width - 32, position.height - 32, 32, 32);
        if( GUI.Button(rbCorner, new GUIContent(_questionMark)) )
        {
            Application.OpenURL("http://www.tmpxyz.com/Skele/docs/81-exporter/77-dae-exporter-manual?showall=&limitstart=");
        }
    }

    void OnLostFocus()
    {
        _CheckRootCurves();
    }

    void OnFocus()
    {
        _CheckRootCurves();
    }

    #endregion "public method"

	#region "private method"
    
    private void _OnGUI_SMR()
    {
        m_RootBone = EditorGUILayout.ObjectField(new GUIContent("Top GO", "fill the topmost GameObject of model"), m_RootBone, typeof(Transform), true) as Transform;
        GUIUtil.PushGUIEnable(m_RootBone != null);
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(60f);
            if (EUtil.Button("AutoFind", "Automatically collect all SMR & MF on this model", Color.blue))
            {
                _AutoFindRenderers();
            }
            GUILayout.Space(60f);
            GUILayout.EndHorizontal();
        }
        GUIUtil.PopGUIEnable();

        EUtil.DrawSplitter();

        //SMR
        for (int idx = 0; idx < m_SMRs.Count; ++idx)
        {
            GUILayout.BeginHorizontal();
            
            if (EUtil.Button("X", "delete", Color.red, GUILayout.Width(30f)))
            {
                m_SMRs.RemoveAt(idx);
                --idx;
                continue;
            }

            Color oc = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            m_SMRs[idx] = EditorGUILayout.ObjectField(m_SMRs[idx], typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
            GUI.backgroundColor = oc;

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        if (GUILayout.Button(new GUIContent("Add SMR Entry", "manually add \"Skinned-Mesh Renderer\"")))
        {
            m_SMRs.Add(null);
        }
        GUILayout.Space(60f);
        GUILayout.EndHorizontal();

        EUtil.DrawSplitter();

        //MF
        for (int idx = 0; idx < m_MFs.Count; ++idx)
        {
            GUILayout.BeginHorizontal();
            if (EUtil.Button("X", "delete", Color.red, GUILayout.Width(30f)))
            {
                m_MFs.RemoveAt(idx);
                --idx;
                continue;
            }

            Color oc = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            m_MFs[idx] = EditorGUILayout.ObjectField(m_MFs[idx], typeof(MeshFilter), true) as MeshFilter;
            GUI.backgroundColor = oc;

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        if (GUILayout.Button(new GUIContent("Add MF Entry", "manually add \"Mesh Filter\"")))
        {
            m_MFs.Add(null);
        }
        GUILayout.Space(60f);
        GUILayout.EndHorizontal();

        EUtil.DrawSplitter();

        //export clips
        EditorGUI.BeginChangeCheck();
        for( int idx=0; idx<m_Clips.Count; ++idx)
        {
            GUILayout.BeginHorizontal();
            if (EUtil.Button("X", "delete", Color.red, GUILayout.Width(30f)))
            {
                m_Clips.RemoveAt(idx);
                --idx;
                continue;
            }
            m_Clips[idx] = EditorGUILayout.ObjectField(m_Clips[idx], typeof(AnimationClip), true) as AnimationClip;
            GUILayout.EndHorizontal();
        }        
        if( EditorGUI.EndChangeCheck() )
        {
            _CheckRootCurves();
        }

        // clips with rootcurve
        if( m_ClipsWithRootCurve.Count > 0 )
        {
            if( ! m_ForceIgnoreRootCurve )
            {
                var info = string.Format("Detected curves on root-transform!\nExported archives will add extra-root node to hold the curves.\n\nClips:\n{0}", Misc.ListToString(m_ClipsWithRootCurve, x => x.name));
                EditorGUILayout.HelpBox(info, MessageType.Info);
            }
            m_ForceIgnoreRootCurve = EditorGUILayout.Toggle(new GUIContent("Ignore Curves on Root", "If true, the curves on root will be discarded in exported archives"), m_ForceIgnoreRootCurve);
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        if (GUILayout.Button(new GUIContent("Add Clip", "add another animation clip to export")))
        {
            m_Clips.Add(null);
        }
        GUILayout.Space(60f);
        GUILayout.EndHorizontal();

        EUtil.DrawSplitter();

        bool bHasValidEntry = _HasValidEntry(); 
        Color c = (bHasValidEntry) ? Color.green : Color.red;
        EUtil.PushBackgroundColor(c);
        GUIUtil.PushGUIEnable(bHasValidEntry);
        GUILayout.BeginHorizontal();
        GUILayout.Space(60f);
        if (GUILayout.Button("Export!"))
        {
            string saveDir = _GetSaveDirectory();
            string filePath = EditorUtility.SaveFilePanel("Select export file path", saveDir, "export_"+m_RootBone.name, "dae");
            if (filePath.Length > 0)
            {
                string recDir = System.IO.Path.GetDirectoryName(filePath);
                _RecordSaveDirectory(recDir);

                SkinnedMeshRenderer[] smrArr = m_SMRs.TakeWhile(x => x != null).ToArray();
                MeshFilter[] mfArr = m_MFs.TakeWhile(x => x != null).ToArray();
                m_Clips.RemoveAll(x => x == null);

                DaeExporter exp = new DaeExporter(smrArr, mfArr, m_RootBone);
                exp.ignoreRootCurves = m_ForceIgnoreRootCurve;
                exp.Export(m_Clips, filePath);

                AssetDatabase.Refresh();
            }
            else
            {
                EUtil.ShowNotification("Export Cancelled...");
            }
        }
        GUILayout.Space(60f);
        GUILayout.EndHorizontal();
        GUIUtil.PopGUIEnable();
        EUtil.PopBackgroundColor();
    }

    private void _CheckRootCurves()
    {
        DaeExporter.FindClipsWithRootCurves(m_Clips, m_ClipsWithRootCurve);
    }

    private bool _HasValidEntry()
    {
        bool bReady4SMR = m_RootBone != null && m_SMRs.Count(x => x != null) > 0;
        bool bReady4MF = /*m_RootBone == null &&*/ m_SMRs.Count(x => x != null) == 0 && m_MFs.Count(x => x != null) > 0;
        return bReady4MF || bReady4SMR;
    }

    private void _AutoFindRenderers()
    {
        // first get to AnimRoot
        Transform tr = m_RootBone;
        while( tr != null && tr.GetComponent<Animation>() == null && tr.GetComponent<Animator>() == null )
        {
            tr = tr.parent;
        }
        if( tr == null )
        {
            Dbg.LogWarn("DaeExporterEditor._AutoFindRenderers: cannot find GO with Animation/Animator in ancestors of {0}", m_RootBone.name);
            return;
        }

        // then recursively find out all SMRs
        SkinnedMeshRenderer[] smrs = tr.GetComponentsInChildren<SkinnedMeshRenderer>();
        m_SMRs.Clear();
        m_SMRs.AddRange(smrs);

        // and recursively find out all MFs
        MeshFilter[] mfs = tr.GetComponentsInChildren<MeshFilter>();
        m_MFs.Clear();
        m_MFs.AddRange(mfs);
    }

    private string _GetSaveDirectory()
    {
        string dir = EditorPrefs.GetString(EDITOR_PREF_KEY_SAVEDIR);
        if( string.IsNullOrEmpty(dir) )
        {
            dir = Application.dataPath;
        }

        return dir;
    }

    private void _RecordSaveDirectory(string newDir)
    {
        EditorPrefs.SetString(EDITOR_PREF_KEY_SAVEDIR, newDir);
    }


    private void _AutoInit()
    {
        Transform tr = Selection.activeTransform;
        if (tr != null)
        {
            Animator ator = tr.GetComponentInParent<Animator>();
            if (ator != null)
            {
                m_RootBone = ator.transform;
                _AutoFindRenderers();
            }
        }
    }

    // private method

    #endregion "private method"

    #region "constant data"
    // constant data

    //public enum OpType
    //{
    //    SMR,
    //    MF,
    //}

    private const string EDITOR_PREF_KEY_SAVEDIR = "__MH_Skele_ExportDAE_Dir";

    #endregion "constant data"
}
