using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChessAPI.Controllers
{
    public class Version
    {
        public string name = "chessAPI";
        public string version = "0.2";
    }

    public class VersionController : ApiController
    {
        public Version GetVersion ()
        {
            Version version = new Version();
            return version;
        }
    }
}
