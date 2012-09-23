using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace CiteSet.Service
{
	/// <summary>
	/// Required installer class.
	/// </summary>
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		/// <summary>
		/// Create a new instance of the ProjectInstaller.
		/// </summary>
		public ProjectInstaller()
		{
			InitializeComponent();
		}
	}
}
