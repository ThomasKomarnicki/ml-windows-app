using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NancyML;

namespace MLService {
    public partial class Service1 : ServiceBase
    {

        private NancyServer _server;

        public Service1() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            _server = new NancyServer();

        }

        protected override void OnStop() {
        }

    }
}
