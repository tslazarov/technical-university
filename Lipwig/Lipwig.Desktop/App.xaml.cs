using Lipwig.Data;
using Lipwig.Data.Contracts;
using Lipwig.Desktop.IocModules;
using Lipwig.Services;
using Lipwig.Services.Contracts;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Lipwig.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureContainer();
            ComposeObjects();
            Current.MainWindow.Show();
        }

        private void ConfigureContainer()
        {
            this.container = new StandardKernel();
            RegisterBindings(this.container);
        }

        private void ComposeObjects()
        {
            Current.MainWindow = this.container.Get<MainWindow>();
        }

        private void RegisterBindings(IKernel kernel)
        {
            kernel.Load(new DataModule());
            kernel.Load(new ServicesModule());
        }
    }
}
