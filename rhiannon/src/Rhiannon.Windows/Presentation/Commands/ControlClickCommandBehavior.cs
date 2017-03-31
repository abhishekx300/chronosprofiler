﻿using System.Windows;
using System.Windows.Controls;

namespace Rhiannon.Windows.Presentation.Commands
{
	public class ControlClickCommandBehavior : ControlEventCommandBehavior
	{
		public ControlClickCommandBehavior(Control control)
			: base(control)
		{

		}

		public override string EventName
		{
			get { return "Click"; }
		}

		protected override string HandlerMethodName
		{
			get { return "OnClick"; }
		}

		public void OnClick(object sender, RoutedEventArgs e)
		{
			ExecuteCommand();
		}
	}
}
