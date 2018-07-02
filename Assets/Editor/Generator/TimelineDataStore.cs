﻿using System.IO;
using System.Linq;
using CAFU.Generator;
using CAFU.Generator.Enumerates;
using JetBrains.Annotations;
using UnityEditor;

namespace CAFU.Timeline.Generator
{
    [UsedImplicitly]
    public class TimelineDataStore : ClassStructureBase
    {
        private const string StructureName = "Data/DataStore/TimelineDataStore";

        public override string Name { get; } = StructureName;

        protected override ParentLayerType ParentLayerType { get; } = ParentLayerType.Data;

        protected override LayerType LayerType { get; } = LayerType.DataStore;

        protected override string ModuleName { get; } = "umm@cafu_generator_extensions_timeline";

        private int CurrentSceneNameIndex { get; set; }

        public TimelineDataStore()
        {
        }

        public TimelineDataStore(int currentSceneNameIndex)
        {
            CurrentSceneNameIndex = currentSceneNameIndex;
        }

        public override void OnGUI()
        {
            base.OnGUI();
            CurrentSceneNameIndex = EditorGUILayout.Popup("Scene Name", CurrentSceneNameIndex, GeneratorWindow.SceneNameList.ToArray());
        }

        public override void Generate(bool overwrite)
        {
            var parameter = new Parameter()
            {
                ParentLayerType = ParentLayerType,
                LayerType = LayerType,
                ClassName = GeneratorWindow.SceneNameList[CurrentSceneNameIndex],
                SceneName = GeneratorWindow.SceneNameList[CurrentSceneNameIndex],
                Overwrite = overwrite,
            };
            parameter.Namespace = CreateNamespace(parameter);

            parameter.UsingList.Add("CAFU.Timeline.Data.DataStore");
            parameter.UsingList.Add($"TimelineName = {this.CreateNamespacePrefix()}Application.Enumerate.TimelineName.{parameter.SceneName}");
            parameter.UsingList.Add($"TimelineEntity = {this.CreateNamespacePrefix()}Data.Entity.TimelineEntity.{parameter.SceneName}");
            parameter.BaseClassName = "TimelineDataStore<TimelineName, TimelineEntity>";

            var generator = new ScriptGenerator(parameter, CreateTemplatePath(TemplateType.Class, StructureName));

            generator.Generate(CreateOutputPath(parameter));
        }

        protected override string CreateNamespace(Parameter parameter)
        {
            return $"{this.CreateNamespacePrefix()}{ParentLayerType.ToString()}.{LayerType.ToString()}.TimelineDataStore";
        }

        protected override string CreateOutputPath(Parameter parameter)
        {
            return Path.Combine(UnityEngine.Application.dataPath, OutputDirectory, parameter.ParentLayerType.ToString(), parameter.LayerType.ToString(), "TimelineDataStore", $"{parameter.ClassName}{ScriptExtension}");
        }
    }
}
