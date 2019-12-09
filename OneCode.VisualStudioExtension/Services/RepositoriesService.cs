﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Newtonsoft.Json;
using OneCode.Core;
using OneCode.Core.Settings;
using OneCode.VsExtension.Utilities;

namespace OneCode.VsExtension.Services
{
    [Export]
    public class RepositoriesService
    {
        public Repositories Repositories { get; set; } = new Repositories();

        public OneCodeSettings Settings
        {
            get => File.Exists(OneCodeSettings.DefaultPath)
                ? JsonConvert.DeserializeObject<OneCodeSettings>(File.ReadAllText(OneCodeSettings.DefaultPath))
                : new OneCodeSettings();
            set => File.WriteAllText(OneCodeSettings.DefaultPath, JsonConvert.SerializeObject(value, Formatting.Indented));
        }

        public void LoadFromSettings()
        {
            Repositories.Load(Settings);
        }

        public void AddAndSaveSettings(RepositorySettings settings)
        {
            Repositories.Add(settings);

            Settings = Repositories.Settings;
        }

        public void RemoveAndSaveSettings(RepositorySettings settings)
        {
            Repositories.Remove(settings);

            Settings = Repositories.Settings;
        }

        public void AddProjectItem(CodeFile file, Class @class, Method method, bool openAfterAdd = false)
        {
            if (file == null)
            {
                return;
            }

            var dte = this.GetDte();
            var project = dte.GetActiveProject();

            //var solution = (IVsSolution)Package.GetGlobalService(typeof(SVsSolution));
            //IVsHierarchy hierarchy;
            //solution.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

            var projectDirectory = project.GetDirectory();
            var fullPath = Path.Combine(projectDirectory ?? string.Empty, file.RelativePathWithoutTargetFramework);

            file.Code.NamespaceName = project.GetDefaultNamespace() + file.AdditionalNamespace;
            file.Code.Classes = new List<Class> { @class };

            if (method != null)
            {
                file.Code.Classes[0].Methods = new List<Method> { method };
            }

            file.SaveTo(fullPath);

            project.AddItemFromFile(fullPath);

            if (openAfterAdd)
            {
                dte.OpenFileAsText(fullPath);
            }
        }
    }
}