﻿using Chronos.Client;
using Chronos.Extensibility;
using Rhiannon.Resources;
using Rhiannon.Windows.Presentation;

namespace Chronos.Extension.ProfilingTarget.InternetInformationService.Win
{
	public class Extension : IExtension
	{
		private readonly IResourceProvider _resourceProvider;
		private readonly IViewsManager _viewsManager;
		private readonly IProfilingTargetCollection _adapterCollection;
		private readonly IClientApplication _application;

		public Extension(IResourceProvider resourceProvider, IViewsManager viewsManager, IClientApplication application, IProfilingTargetCollection adapterCollection)
		{
			_resourceProvider = resourceProvider;
			_viewsManager = viewsManager;
			_application = application;
			_adapterCollection = adapterCollection;
		}

		public void Initialize()
		{
			_viewsManager.Register<ViewActivator>();
            _resourceProvider.RegisterManager(Resources.Localization.ResourceManager, ResourceType.Text);
            _resourceProvider.RegisterManager(Resources.Images.ResourceManager, ResourceType.Image);
			_adapterCollection.Register(new ProfilingTarget(_viewsManager, _application, _resourceProvider));
		}
	}
}
