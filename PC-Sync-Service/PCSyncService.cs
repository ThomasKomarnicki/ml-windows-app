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

namespace PC_Sync_Service {
    public partial class PCSyncService : ServiceBase {
        public PCSyncService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            NancyServer nancy = new NancyServer();
        }

        protected override void OnStop() {
        }
    }
}
